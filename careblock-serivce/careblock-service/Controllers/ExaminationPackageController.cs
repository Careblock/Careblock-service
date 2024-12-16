using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Web.Examination;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class ExaminationPackageController : BaseController
{
    private readonly IExaminationPackageService _examinationPackageService;
    
    public ExaminationPackageController(IExaminationPackageService examinationPackageService)
    {
        _examinationPackageService = examinationPackageService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<ExaminationPackageDto>>> GetAll()
    {
        var result = await _examinationPackageService.GetAll();
        return new ApiResponse<List<ExaminationPackageDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-type/{examinationTypeId:int}")]
    public async Task<ApiResponse<List<ExaminationPackageResponseDto>>> GetByType([FromRoute] int examinationTypeId)
    {
        var result = await _examinationPackageService.GetByType(examinationTypeId);
        return new ApiResponse<List<ExaminationPackageResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-type-organization/{examinationTypeId:int}/{organizationId:Guid}")]
    public async Task<ApiResponse<List<ExaminationPackageResponseDto>>> GetByTypeAndOrganization([FromRoute] int examinationTypeId, [FromRoute] Guid organizationId)
    {
        var result = await _examinationPackageService.GetByTypeAndOrganization(examinationTypeId, organizationId);
        return new ApiResponse<List<ExaminationPackageResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-organization/{userId:Guid}")]
    public async Task<ApiResponse<List<ExaminationPackageResponseDto>>> GetByOrganization([FromRoute] Guid userId)
    {
        var result = await _examinationPackageService.GetByOrganization(userId);
        return new ApiResponse<List<ExaminationPackageResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<ExaminationPackage>> Get([FromRoute] Guid id)
    {
        var result = await _examinationPackageService.GetById(id);
        return new ApiResponse<ExaminationPackage>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create/{userId}")]
    public async Task<ApiResponse<Guid>> Create([FromForm] ExaminationPackageFormDto examinationType, [FromRoute] Guid userId)
    {
        var result = await _examinationPackageService.Create(examinationType, userId);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> CreateNew([FromForm] ExaminationPackageFormDto examinationType)
    {
        var result = await _examinationPackageService.CreateNew(examinationType);
        return new ApiResponse<Guid>(result);
    }

    #endregion


    #region PUT

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<ExaminationPackageDto>> Update([FromRoute] Guid id, [FromForm] ExaminationPackageFormDto examinationPackageFormDto)
    {
        var result = await _examinationPackageService.Update(id, examinationPackageFormDto);
        return new ApiResponse<ExaminationPackageDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _examinationPackageService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}