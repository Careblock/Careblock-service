using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Shared.Enum;

namespace careblock_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<AppointmentDto>>> GetAll()
    {
        var result = await _appointmentService.GetAll();
        return new ApiResponse<List<AppointmentDto>>(result, true);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ApiResponse<AppointmentDto?>> Get(Guid id)
    {
        bool isSucccess = true;
        var result = await _appointmentService.GetById(id);
        if (Guid.Equals(result.Id, Guid.Empty))
        {
            isSucccess = false;
            result = null;
        }
        return new ApiResponse<AppointmentDto?>(result, isSucccess);
    }

    [HttpGet]
    [Route("get-by-patient/{patientId:guid}")]
    public async Task<ApiResponse<List<AppointmentHistoryDto>?>> GetByPatientID(Guid patientId)
    {
        var result = await _appointmentService.GetByPatientID(patientId);
        return new ApiResponse<List<AppointmentHistoryDto>?>(result, true);
    }

    [HttpGet]
    [Route("update-status/{status}/{id}")]
    public async Task<ApiResponse<bool>> UpdateStatus(AppointmentStatus status, Guid id)
    {
        var result = await _appointmentService.UpdateStatus(status, id);
        return new ApiResponse<bool>(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<Guid>> Create([FromBody] AppointmentFormDto appointment)
    {
        var result = await _appointmentService.Create(appointment);
        return new ApiResponse<Guid>(result);
    }

    [HttpPost]
    [Route("update")]
    public async Task<ApiResponse<bool>> Update(AppointmentDto appointment)
    {
        var result = await _appointmentService.Update(appointment);
        return new ApiResponse<bool>(result);
    }

    [HttpDelete]
    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        var result = await _appointmentService.Delete(id);
        return new ApiResponse<bool>(result);
    }
}