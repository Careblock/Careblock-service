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
public class ExaminationOptionController : BaseController
{
    private readonly IExaminationOptionService _examinationOptionService;

    public ExaminationOptionController(IExaminationOptionService examinationOptionService)
    {
        _examinationOptionService = examinationOptionService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<ExaminationOptionDto>>> GetAll()
    {
        var result = await _examinationOptionService.GetAll();
        return new ApiResponse<List<ExaminationOptionDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("get-by-package/{appointmentId:guid}")]
    public async Task<ApiResponse<List<ExaminationOption>>> GetByPackage(Guid appointmentId)
    {
        var result = await _examinationOptionService.GetByPackage(appointmentId);
        return new ApiResponse<List<ExaminationOption>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<ExaminationOption>> Get([FromRoute] Guid id)
    {
        var result = await _examinationOptionService.GetById(id);
        return new ApiResponse<ExaminationOption>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] ExaminationOptionFormDto examinationOption)
    {
        var result = await _examinationOptionService.Create(examinationOption);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<ExaminationOptionDto>> Update([FromRoute] Guid id, [FromForm] ExaminationOptionFormDto examinationOption)
    {
        var result = await _examinationOptionService.Update(id, examinationOption);
        return new ApiResponse<ExaminationOptionDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _examinationOptionService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}