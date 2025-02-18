using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class AppointmentController : BaseController
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<AppointmentDto>>> GetAll()
    {
        var result = await _appointmentService.GetAll();
        return new ApiResponse<List<AppointmentDto>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<AppointmentDto?>> Get([FromRoute] Guid id)
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

    [AllowAnonymous]
    [HttpGet("get-by-patient/{patientId:guid}")]
    public async Task<ApiResponse<List<AppointmentHistoryDto>?>> GetByPatientID([FromRoute] Guid patientId)
    {
        var result = await _appointmentService.GetByPatientID(patientId);
        return new ApiResponse<List<AppointmentHistoryDto>?>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("update-status/{status}/{id}")]
    public async Task<ApiResponse<bool>> UpdateStatus([FromRoute] AppointmentStatus status, [FromRoute] Guid id)
    {
        var result = await _appointmentService.UpdateStatus(status, id);
        return new ApiResponse<bool>(result);
    }

    [AllowAnonymous]
    [HttpGet("not-assigned/{userId:guid}")]
    public async Task<ApiResponse<int>> GetNumberNotAssigned([FromRoute] Guid userId)
    {
        var result = await _appointmentService.GetNumberNotAssigned(userId);
        return new ApiResponse<int>(result, true);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("get-by-organization")]
    public async Task<ApiResponse<AppointmentHistories>> GetByOrganizationID([FromBody] AppointmentRequest appointmentRequest)
    {
        var result = await _appointmentService.GetByOrganizationID(appointmentRequest);
        return new ApiResponse<AppointmentHistories>(result, true);
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromBody] AppointmentFormDto appointment)
    {
        var result = await _appointmentService.Create(appointment);
        if (Guid.Equals(result, Guid.Empty)) return new ApiResponse<Guid>(result, false);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPost("assign-doctor/{appointmentId:guid}")]
    public async Task<ApiResponse<int>> AssignDoctor([FromRoute] Guid appointmentId, [FromBody] NotAssignedRequest request)
    {
        var result = await _appointmentService.AssignDoctor(appointmentId, request);
        return new ApiResponse<int>(result);
    }

    [AllowAnonymous]
    [HttpPost("update")]
    public async Task<ApiResponse<bool>> Update([FromBody] AppointmentDto appointment)
    {
        var result = await _appointmentService.Update(appointment);
        return new ApiResponse<bool>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _appointmentService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}