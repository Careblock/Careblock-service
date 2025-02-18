using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class AppointmentDetailController : BaseController
{
    private readonly IAppointmentDetailService _appointmentDetailService;

    public AppointmentDetailController(IAppointmentDetailService appointmentDetailService)
    {
        _appointmentDetailService = appointmentDetailService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<AppointmentDetailDto>>> GetAll()
    {
        var result = await _appointmentDetailService.GetAll();
        return new ApiResponse<List<AppointmentDetailDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<AppointmentDetailDto?>> Get([FromRoute] Guid id)
    {
        bool isSucccess = true;
        var result = await _appointmentDetailService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<AppointmentDetailDto?>(result, isSucccess);
    }

    [AllowAnonymous]
    [HttpGet("get-by-appointment/{appointmentId:guid}")]
    public async Task<ApiResponse<AppointmentDetailDto?>> GetByAppointmentId([FromRoute] Guid appointmentId)
    {
        bool isSucccess = true;
        var result = await _appointmentDetailService.GetByAppointmentId(appointmentId);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<AppointmentDetailDto?>(result, isSucccess);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] AppointmentDetailFormDto appointment)
    {
        var result = await _appointmentDetailService.Create(appointment);
        if (Guid.Equals(result, Guid.Empty)) return new ApiResponse<Guid>(result, false);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPost("update")]
    public async Task<ApiResponse<bool>> Update([FromBody] AppointmentDetailDto appointment)
    {
        var result = await _appointmentDetailService.Update(appointment);
        return new ApiResponse<bool>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _appointmentDetailService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}