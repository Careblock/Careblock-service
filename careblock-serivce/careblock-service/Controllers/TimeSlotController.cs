using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.TimeSlot;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class TimeSlotController : BaseController
{
    private readonly ITimeSlotService _timeSlotService;

    public TimeSlotController(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<TimeSlotResponseDto>>> GetAll()
    {
        var result = await _timeSlotService.GetAll();
        return new ApiResponse<List<TimeSlotResponseDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<TimeSlot>> Get([FromRoute] Guid id)
    {
        var result = await _timeSlotService.GetById(id);
        return new ApiResponse<TimeSlot>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] TimeSlotFormDto timeSlot)
    {
        var result = await _timeSlotService.Create(timeSlot);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<TimeSlotResponseDto>> Update([FromRoute] Guid id, [FromForm] TimeSlotFormDto timeSlot)
    {
        var result = await _timeSlotService.Update(id, timeSlot);
        return new ApiResponse<TimeSlotResponseDto>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _timeSlotService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}