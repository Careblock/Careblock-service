using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.MedicalService;
using Careblock.Model.Web.Diagnostic;

namespace careblock_service.Controllers;

[ApiController]
[Route("[controller]")]
public class DiagnosticController : BaseController
{
    private readonly IDiagnosticService _diagnosticService;

    public DiagnosticController(IDiagnosticService diagnosticService)
    {
        _diagnosticService = diagnosticService;
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<DiagnosticDto>>> GetAll()
    {
        var result = await _diagnosticService.GetAll();
        return new ApiResponse<List<DiagnosticDto>>(result, true);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ApiResponse<DiagnosticDto?>> Get(Guid id)
    {
        bool isSucccess = true;
        var result = await _diagnosticService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<DiagnosticDto?>(result, isSucccess);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<Guid>> Create(DiagnosticFormDto diagnostic)
    {
        var result = await _diagnosticService.Create(diagnostic);
        return new ApiResponse<Guid>(result);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ApiResponse<bool>> Update(DiagnosticDto diagnostic)
    {
        var result = await _diagnosticService.Update(diagnostic);
        return new ApiResponse<bool>(result);
    }

    [HttpDelete]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var result = await _diagnosticService.Delete(id);
        return new ApiResponse<bool>(result);
    }
}