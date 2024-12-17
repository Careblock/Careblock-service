using Careblock.Model.Database;
using Careblock.Model.Web.Notification;

namespace Careblock.Service.BusinessLogic.Interface;

public interface INotificationService
{
    Task<List<Notification>> GetAll();
    Task<Notification> GetById(Guid id); 
    Task<List<Notification>> GetByUserId(Guid userId);
    Task<Guid> Create(NotificationDto notification);
    Task<Notification> UpdateIsRead(Guid id); 
    Task<Notification> Update(Guid id, Notification notification); 
    Task<bool> Delete(Guid id);
}