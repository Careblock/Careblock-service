using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database; 
using careblock_service.Authorization;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Result;
using Careblock.Model.Shared.Enum;
using Careblock.Service.BusinessLogic;

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

    [AllowAnonymous]
    [HttpGet("get-bill/{appointmentId:guid}")]
    public async Task<ApiResponse<BillDto>> GetBill([FromRoute] Guid appointmentId)
    {
        var result = await _resultService.GetBill(appointmentId);
        return new ApiResponse<BillDto>(result);
    }

    [AllowAnonymous]
    [HttpGet("send/{id}")]
    public async Task<ApiResponse<bool>> Send([FromRoute] Guid id)
    {
        var result = await _resultService.Send(id);
        return new ApiResponse<bool>(result, true);
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

    [AllowAnonymous]
    [HttpPost("sign")]
    public async Task<ApiResponse<bool>> Sign([FromBody] SignResultInputDto input)
    {
        var rs = await _resultService.Sign(input);
        return new ApiResponse<bool>(rs, true);
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