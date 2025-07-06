using careblock_service.Authorization;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using careblock_service.ResponseModel;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants .ADMIN})]
[ApiController]
[Route("[controller]")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;
    private readonly IOrganizationService _organizationService;

    public AccountController(IAccountService accountService, IOrganizationService organizationService)
    {
        _accountService = accountService;
        _organizationService = organizationService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<AccountDto?>> Get([FromRoute] Guid id)
    {
        bool isSucccess = true;
        var result = await _accountService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        } else
        {
            var organization = await _organizationService.GetByUserId(id);
            result.Organization = organization;
        }
        return new ApiResponse<AccountDto?>(result, isSucccess);
    }

    [AllowAnonymous]
    [HttpGet("get-all-doctor")]
    public async Task<ApiResponse<List<DoctorDto>>> GetAllDoctor()
    {
        var result = await _accountService.GetAllDoctor();
        return new ApiResponse<List<DoctorDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-doctors-org/{place}/{doctorID}")]
    public async Task<ApiResponse<List<DoctorDto>>> GetDoctorsOrg([FromRoute] Place place, [FromRoute] Guid doctorID)
    {
        var result = await _accountService.GetDoctorsOrg(place, doctorID);
        return new ApiResponse<List<DoctorDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-managers-org/{organizationId}")]
    public async Task<ApiResponse<List<DoctorDto>>> GetManagersOrg([FromRoute] Guid organizationId)
    {
        var result = await _accountService.GetManagersOrg(organizationId);
        return new ApiResponse<List<DoctorDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-scheduled-patient/{status}/{doctorID}")]
    public async Task<ApiResponse<List<PatientDto>>> GetScheduledPatient([FromRoute] AppointmentStatus status, [FromRoute] Guid doctorID)
    {
        var result = await _accountService.GetScheduledPatient(status, doctorID);
        return new ApiResponse<List<PatientDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-default-data/{appointmentId}")]
    public async Task<ApiResponse<DataDefaultDto>> GetDataDefault([FromRoute] Guid appointmentId)
    {
        var result = await _accountService.GetDataDefault(appointmentId);
        var patient = await _accountService.GetById(result.PatientId.GetValueOrDefault());
        result.DateOfBirth = patient.DateOfBirth.GetValueOrDefault();
        return new ApiResponse<DataDefaultDto>(result, true);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("remove-doctors-org/{doctorID}")]
    public async Task<ApiResponse<bool>> RemoveDoctorFromOrg([FromRoute] Guid doctorID)
    {
        var result = await _accountService.RemoveDoctorFromOrg(doctorID);
        return new ApiResponse<bool>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("grant-permission/{userID}")]
    public async Task<ApiResponse<bool>> GrantPermission([FromRoute] Guid userID, [FromBody] PermissionRequest permissions)
    {
        var result = await _accountService.GrantPermission(userID, permissions.Permissions);
        return new ApiResponse<bool>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("grant-sign-permission")]
    public async Task<ApiResponse<bool>> GrantSignPermission([FromBody] GrantSignRequest grantSignRequest)
    {
        var result = await _accountService.GrantSignPermission(grantSignRequest);
        return new ApiResponse<bool>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("has-account")]
    public async Task<ApiResponse<bool>> HasAccount([FromQuery] string stakeId)
    {
        var result = await _accountService.HasAccount(stakeId);
        return new ApiResponse<bool>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("choose-department/{userId:guid}")]
    public async Task<ApiResponse<bool>> ChooseDepartment([FromRoute] Guid userId, [FromBody] ChooseDepartmentRequest request)
    {
        var result = await _accountService.ChooseDepartment(userId, request);
        return new ApiResponse<bool>(result, true);
    }

    #endregion


    #region PUT

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<AccountDto>> Update([FromRoute] Guid id, [FromForm] AccountRequest account)
    {
        var result = await _accountService.Update(id, account);
        return new ApiResponse<AccountDto>(result, true);
    }

    #endregion


    #region JWT & Refreshtoken

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ApiResponse<Guid>> Register([FromForm] AccountFormDto model)
    { 
        return new ApiResponse<Guid>(await _accountService.Register(model),true);
    }
    
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ApiResponse<AccountDto>> Authenticate([FromBody] AuthenticationRequest request)
    {
        var response = await _accountService.Authenticate(request, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return new ApiResponse<AccountDto>(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ApiResponse<AccountDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _accountService.RefreshToken(refreshToken ?? "", IpAddress());
        SetTokenCookie(response.RefreshToken);
        return new ApiResponse<AccountDto>(response);
    }

    [AllowAnonymous]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] string token)
    {
        // accept token from request body or cookie
        var curToken = token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        // users can revoke their own tokens and admins can revoke any tokens
        if (!Account.OwnsToken(token) && !Account.GetRoleNames().Where(x => string.Equals(x, Constants.ADMIN)).Any())
            return Unauthorized(new { message = "Unauthorized" });

        var result = await _accountService.RevokeRefreshToken(token, IpAddress());
        return Ok(new { message = "Token revoked" });
    }

    #endregion


    #region Dummy functions

    [AllowAnonymous] 
    [HttpGet("test-anonymous")]
    public string TestAnonymous()
    {
        return "hello annoy";
    }
    
    [HttpGet("test-all-roles")]
    public string TestAuthenticate()
    {
        return "hello user auth";
    }

    [AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT })]
    [HttpGet("test-doctor-and-patient")]
    public string TestDoctorOnly()
    {
        return "hello doctor all";
    }

    [AdminAuthorize(new string[] { Constants.ADMIN })]
    [HttpGet("admin")]
    public string TestAdminOnly()
    {
        return "hello admin";
    }

    #endregion
    

    #region private Functions

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "";
        }
    }

    #endregion
}