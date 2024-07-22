using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Model.Web.Organization;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;

namespace careblock_service.Controllers;

// [Authorize(new Role[]{Role.Admin})]
[ApiController]
[Route("[controller]")]
public class OrganizationController : BaseController
{
    private readonly IOrganizationService _organizationService;
    
    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }
    
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<OrganizationDto>>> GetAll()
    {
        var result = await _organizationService.GetAll();
        return new ApiResponse<List<OrganizationDto>>(result, true);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ApiResponse<OrganizationDto>> Get(Guid id)
    {
        var result = await _organizationService.GetById(id);
        return new ApiResponse<OrganizationDto>(result);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<Guid>> Create(OrganizationFormDto organization)
    {
        var result = await _organizationService.Create(organization);
        return new ApiResponse<Guid>(result);
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<ApiResponse<bool>> Update(OrganizationDto organization)
    {
        var result = await _organizationService.Update(organization);
        return new ApiResponse<bool>(result);
    }
    
    [HttpDelete]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var result = await _organizationService.Delete(id);
        return new ApiResponse<bool>(result);
    }
}