using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Model.Web.Payment;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class PaymentMethodController : BaseController
{
    private readonly IPaymentMethodService _paymentMethodService;
    
    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<PaymentMethod>>> GetAll()
    {
        var result = await _paymentMethodService.GetAll();
        return new ApiResponse<List<PaymentMethod>>(result, true);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<PaymentMethod>> Get([FromRoute] Guid id)
    {
        var result = await _paymentMethodService.GetById(id);
        return new ApiResponse<PaymentMethod>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<int>> Create([FromForm] PaymentMethodFormDto paymentMethod)
    {
        var result = await _paymentMethodService.Create(paymentMethod);
        return new ApiResponse<int>(result);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<PaymentMethod>> Update([FromRoute] int id, [FromForm] PaymentMethodFormDto paymentMethod)
    {
        var result = await _paymentMethodService.Update(id, paymentMethod);
        return new ApiResponse<PaymentMethod>(result);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] int id)
    {
        var result = await _paymentMethodService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}