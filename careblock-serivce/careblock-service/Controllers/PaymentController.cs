using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Payment;
using Careblock.Service.BusinessLogic;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;
    
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<Payment>>> GetAll()
    {
        var result = await _paymentService.GetAll();
        return new ApiResponse<List<Payment>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<Payment>> Get([FromRoute] Guid id)
    {
        var result = await _paymentService.GetById(id);
        return new ApiResponse<Payment>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-paid-by-appointment/{appointmentId:guid}")]
    public async Task<ApiResponse<Payment>> GetPaidByAppointmentId([FromRoute] Guid appointmentId)
    {
        var result = await _paymentService.GetPaidByAppointmentId(appointmentId);
        return new ApiResponse<Payment>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromForm] PaymentFormDto payment)
    {
        var result = await _paymentService.Create(payment);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<Payment>> Update([FromRoute] Guid id, [FromForm] PaymentFormDto payment)
    {
        var result = await _paymentService.Update(id, payment);
        return new ApiResponse<Payment>(result);
    }

    [AllowAnonymous]
    [HttpPut("update-by-appointment/{appointmentId}")]
    public async Task<ApiResponse<Guid>> UpdateByAppointment([FromRoute] Guid appointmentId, [FromForm] PaymentFormDto payment)
    {
        var result = await _paymentService.UpdateByAppointment(appointmentId, payment);
        return new ApiResponse<Guid>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _paymentService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}