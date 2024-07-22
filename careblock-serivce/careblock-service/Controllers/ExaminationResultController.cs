using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;

namespace careblock_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ExaminationResultController : BaseController
{
    private readonly IExaminationResultService _examinationResultService;

    public ExaminationResultController(IExaminationResultService examinationResultService)
    {
        _examinationResultService = examinationResultService;
    }

    [HttpGet]
    [Route("get-file-by-patient/{patientId:guid}")]
    public async Task<IActionResult?> GetFileByPatientID(Guid patientId)
    {
        var result = await _examinationResultService.GetFileByPatientID(patientId);
        return File(result, "application/pdf", "examination-result.pdf");
    }

    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] ExaminationResultFormDto result)
    {
        var rs = await _examinationResultService.Create(result);
        return new ApiResponse<Guid>(rs);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ApiResponse<bool>> Update(ExaminationResultDto result)
    {
        var rs = await _examinationResultService.Update(result);
        return new ApiResponse<bool>(rs);
    }

    [HttpDelete]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var result = await _examinationResultService.Delete(id);
        return new ApiResponse<bool>(result);
    }
}