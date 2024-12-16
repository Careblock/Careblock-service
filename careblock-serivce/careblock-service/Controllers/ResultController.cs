using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database; 
using careblock_service.Authorization;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class ResultController : BaseController
{
    private readonly IResultService _resultService;

    public ResultController(IResultService resultService)
    {
        _resultService = resultService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-by-appointment/{appointmentId:guid}")]
    public async Task<ApiResponse<List<Result>>> GetByAppointment([FromRoute] Guid appointmentId)
    {
        var result = await _resultService.GetByAppointment(appointmentId);
        return new ApiResponse<List<Result>>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] ResultFormDto result)
    {
        var rs = await _resultService.Create(result);
        return new ApiResponse<Guid>(rs);
    }

    [AllowAnonymous]
    [HttpPost("update")]
    public async Task<ApiResponse<bool>> Update([FromBody] ResultDto result)
    {
        var rs = await _resultService.Update(result);
        return new ApiResponse<bool>(rs);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _resultService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}