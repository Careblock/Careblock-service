using careblock_service.Authorization;
using careblock_service.ResponseModel;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Careblock.Model.Shared.Common;
using Careblock.Model.Database;
using Careblock.Model.Web.Notification;

namespace careblock_service.Controllers;

[AdminAuthorize(new string[] { Constants.DOCTOR, Constants.PATIENT, Constants.MANAGER, Constants.ADMIN })]
[ApiController]
[Route("[controller]")]
public class NotificationController : BaseController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    #region GET

    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ApiResponse<List<Notification>>> GetAll()
    {
        var result = await _notificationService.GetAll();
        return new ApiResponse<List<Notification>>(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ApiResponse<Notification>> Get([FromRoute] Guid id)
    {
        var result = await _notificationService.GetById(id);
        return new ApiResponse<Notification>(result);
    }

    [AllowAnonymous]
    [HttpGet("get-by-user/{userId:guid}")]
    public async Task<ApiResponse<List<Notification>>> GetByUserId([FromRoute] Guid userId)
    {
        var result = await _notificationService.GetByUserId(userId);
        return new ApiResponse<List<Notification>>(result);
    }

    #endregion


    #region POST

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ApiResponse<Guid>> Create([FromBody] NotificationDto notification)
    {
        var result = await _notificationService.Create(notification);
        return new ApiResponse<Guid>(result);
    }

    [AllowAnonymous]
    [HttpPost("decline/{id:guid}")]
    public async Task<ApiResponse<bool>> Decline([FromRoute] Guid id)
    {
        var result = await _notificationService.Delete(id);
        return new ApiResponse<bool>(result, result);
    }

    #endregion


    #region PUT

    [AllowAnonymous]
    [HttpPut("update-read/{id:guid}")]
    public async Task<ApiResponse<Notification>> UpdateIsRead([FromRoute] Guid id)
    {
        var result = await _notificationService.UpdateIsRead(id);
        return new ApiResponse<Notification>(result, true);
    }

    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ApiResponse<Notification>> Update([FromRoute] Guid id, [FromForm] Notification notification)
    {
        var result = await _notificationService.Update(id, notification);
        return new ApiResponse<Notification>(result, true);
    }

    #endregion


    #region DELETE

    [AllowAnonymous]
    [HttpDelete("{id:guid}")]
    public async Task<ApiResponse<bool>> Delete([FromRoute] Guid id)
    {
        var result = await _notificationService.Delete(id);
        return new ApiResponse<bool>(result);
    }

    #endregion
}