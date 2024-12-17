using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Model.Web.Organization;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class OrganizationController : BaseController
{
    private readonly IOrganizationService _organizationService;
    
    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }


    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<OrganizationDto>>> GetAll()
    {
        var result = await _organizationService.GetAll();
        return new ApiResponse<List<OrganizationDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<OrganizationDto>> Get([FromRoute] Guid id)
    {
        var result = await _organizationService.GetById(id);
        return new ApiResponse<OrganizationDto>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-by-user/{userId:guid}")]
    public async Task<ApiResponse<OrganizationDto>> GetByUserId([FromRoute] Guid userId)
    {
        var result = await _organizationService.GetByUserId(userId);
        return new ApiResponse<OrganizationDto>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] OrganizationFormDto organization)
    {
        var result = await _organizationService.Create(organization);
        return new ApiResponse<Guid>(result);
    }

    #endregion


    #region PUT
     
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<OrganizationDto>> Update([FromRoute] Guid id, [FromForm] OrganizationFormDto organization)
    {
        var result = await _organizationService.Update(id, organization);
        return new ApiResponse<OrganizationDto>(result, true);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _organizationService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}