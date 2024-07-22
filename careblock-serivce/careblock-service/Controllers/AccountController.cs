using careblock_service.Authorization;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using careblock_service.ResponseModel;
using Careblock.Model.Database;

namespace careblock_service.Controllers;

[Authorize(new Role[]{Role.Doctor, Role.Patient, Role.DoctorManager, Role.Admin})]
[ApiController]
[Route("[controller]")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id:guid}")]
    public async Task<ApiResponse<AccountDto?>> Get(Guid id)
    {
        bool isSucccess = true;
        var result = await _accountService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<AccountDto?>(result, isSucccess);
    }

    #region JWT & Refreshtoken

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ApiResponse<Guid>> Register([FromForm] AccountFormDto model)
    { 
        return new ApiResponse<Guid>(await _accountService.Register(model),true);
    }
    
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ApiResponse<AccountDto>> Authenticate([FromBody] string stakeId)
    {
        var response = await _accountService.Authenticate(stakeId, ipAddress());
        setTokenCookie(response.RefreshToken);
        return new ApiResponse<AccountDto>(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ApiResponse<AccountDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _accountService.RefreshToken(refreshToken ?? "", ipAddress());
        setTokenCookie(response.RefreshToken);
        return new ApiResponse<AccountDto>(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] string token)
    {
        // accept token from request body or cookie
        var curToken = token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        // users can revoke their own tokens and admins can revoke any tokens
        if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var result = await _accountService.RevokeRefreshToken(token, ipAddress());
        return Ok(new { message = "Token revoked" });
    }

    #endregion

    [AllowAnonymous]
    [HttpGet("filter-by-organization/{organizationID}")]
    public async Task<ApiResponse<List<DoctorDto>>> FilterByOrganization(Guid organizationID)
    {
        var result = await _accountService.FilterByOrganization(organizationID);
        return new ApiResponse<List<DoctorDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-scheduled-patient/{status}/{doctorID}")]
    public async Task<ApiResponse<List<PatientDto>>> GetScheduledPatient(AppointmentStatus status, Guid doctorID)
    {
        var result = await _accountService.GetScheduledPatient(status, doctorID);
        return new ApiResponse<List<PatientDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("has-account")]
    public async Task<ApiResponse<bool>> HasAccount(string stakeId)
    {
        var result = await _accountService.HasAccount(stakeId);
        return new ApiResponse<bool>(result,true);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<AccountDto>> Update([FromRoute] Guid id, [FromForm] AccountRequest account)
    {
        var result = await _accountService.Update(id, account);
        return new ApiResponse<AccountDto>(result, true);
    }

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
    
    [Authorize(new Role[]{Role.Doctor, Role.Patient})]  
    [HttpGet("test-doctor-and-patient")]
    public string TestDoctorOnly()
    {
        return "hello doctor all";
    }
    
    [Authorize(Role.Admin)]  
    [HttpGet("admin")]
    public string TestAdminOnly()
    {
        return "hello admin";
    }

    #endregion
    
    #region private Functions

    private void setTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string ipAddress()
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