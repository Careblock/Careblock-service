using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.Department;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class DepartmentController : BaseController
{
    private readonly IDepartmentService _departmentService;
    
    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<DepartmentDto>>> GetAll()
    {
        var result = await _departmentService.GetAll();
        return new ApiResponse<List<DepartmentDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<DepartmentDto>> Get([FromRoute] Guid id)
    {
        var result = await _departmentService.GetById(id);
        return new ApiResponse<DepartmentDto>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-by-user/{userId:guid}")]
    public async Task<ApiResponse<List<DepartmentDto>>> GetByUserId([FromRoute] Guid userId)
    {
        var result = await _departmentService.GetByUserId(userId);
        return new ApiResponse<List<DepartmentDto>>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-by-organization/{organizationId:guid}")]
    public async Task<ApiResponse<List<DepartmentDto>>> GetByOrganization([FromRoute] Guid organizationId)
    {
        var result = await _departmentService.GetByOrganization(organizationId);
        return new ApiResponse<List<DepartmentDto>>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromBody] DepartmentFormDto department)
    {
        var result = await _departmentService.Create(department);
        return new ApiResponse<Guid>(result);
    }

    #endregion


    #region PUT

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<bool>> Update([FromRoute] Guid id, [FromBody] DepartmentDto department)
    {
        var result = await _departmentService.Update(id, department);
        return new ApiResponse<bool>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _departmentService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}