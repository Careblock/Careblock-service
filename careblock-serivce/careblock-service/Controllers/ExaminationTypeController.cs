using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class ExaminationTypeController : BaseController
{
    private readonly IExaminationTypeService _examinationTypeService;
    
    public ExaminationTypeController(IExaminationTypeService examinationTypeService)
    {
        _examinationTypeService = examinationTypeService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<ExaminationType>>> GetAll()
    {
        var result = await _examinationTypeService.GetAll();
        return new ApiResponse<List<ExaminationType>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<ExaminationType>> Get([FromRoute] Guid id)
    {
        var result = await _examinationTypeService.GetById(id);
        return new ApiResponse<ExaminationType>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-by-user/{userId:guid}")]
    public async Task<ApiResponse<List<ExaminationType>>> GetByUserId([FromRoute] Guid userId)
    {
        var result = await _examinationTypeService.GetByUserId(userId);
        return new ApiResponse<List<ExaminationType>>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<int>> Create([FromForm] ExaminationTypeFormDto examinationType)
    {
        var result = await _examinationTypeService.Create(examinationType);
        return new ApiResponse<int>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<ExaminationTypeDto>> Update([FromRoute] int id, [FromForm] ExaminationTypeFormDto examinationType)
    {
        var result = await _examinationTypeService.Update(id, examinationType);
        return new ApiResponse<ExaminationTypeDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] int id)
    {
        var result = await _examinationTypeService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}