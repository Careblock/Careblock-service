using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Notification;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class NotificationService : EntityService<Notification>, INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public NotificationService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.NotificationRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<Notification>> GetAll()
    {
        return await _unitOfWork.NotificationRepository.GetAll().Select(x => new Notification()
        {
            Id = x.Id,
            AccountId = x.AccountId,
            CreatedDate = x.CreatedDate,
            IsRead = x.IsRead,
            Link = x.Link,
            Message = x.Message,
            NotificationTypeId = x.NotificationTypeId,
        }).ToListAsync();
    }

    public async Task<Notification> GetById(Guid id)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
        if (notification == null) return new Notification();

        return new Notification()
        {
            Id = notification.Id,
            AccountId = notification.AccountId,
            CreatedDate = notification.CreatedDate,
            IsRead = notification.IsRead,
            Link = notification.Link,
            Message = notification.Message,
            NotificationTypeId = notification.NotificationTypeId,
        };
    }

    public async Task<List<Notification>> GetByUserId(Guid userId)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetAll().Where(x => Guid.Equals(x.AccountId, userId)).OrderByDescending(x => x.CreatedDate).ToListAsync();

        return notifications;
    }

    public async Task<Guid> Create(NotificationDto notification)
    {
        var result = await (from acc in _dbContext.Accounts
                            join dep in _dbContext.Departments on acc.DepartmentId equals dep.Id
                            join org in _dbContext.Organizations on dep.OrganizationId equals org.Id
                            where Guid.Equals(acc.Id, notification.OriginId)
                            select new
                            {
                                dep.OrganizationId,
                                org.Name
                            }).FirstOrDefaultAsync();

        Guid? originId = (notification.NotificationTypeId == (int)NotificationEnum.Invite) ? result?.OrganizationId : null;
        string originName, message = string.Empty;
        if (notification.NotificationTypeId == (int)NotificationEnum.Invite && notification.OriginId != null)
        {
            originName = _dbContext.Accounts.Where(acc => Guid.Equals(acc.Id, notification.OriginId)).FirstAsync().Result.Firstname ?? "Someone";
            message = originName + $" invited you to join {result?.Name} hospital.";
        } else
        {
            message = notification.Message;
        }


        var newNotification = new Notification()
        {
            Id = Guid.NewGuid(),
            AccountId = notification.AccountId,
            NotificationTypeId = notification.NotificationTypeId,
            Link = notification.Link,
            IsRead = false,
            Message = message,
            OriginId = originId,
            CreatedDate = DateTime.Now,
        };

        await CreateAsync(newNotification);
        return newNotification.Id;
    }

    public async Task<Notification> UpdateIsRead(Guid id)
    {
        var noti = await _unitOfWork.NotificationRepository.GetByIdAsync(id) ?? throw new AppException("Notification not found");
        noti.IsRead = true;
        await UpdateAsync(noti);

        return new Notification
        {
            Id = id,
            AccountId = noti.AccountId,
            OriginId = noti.OriginId,
            CreatedDate = noti.CreatedDate,
            IsRead = noti.IsRead,
            Link = noti.Link,
            Message = noti.Message,
            NotificationTypeId = noti.NotificationTypeId,
        };
    }

    public async Task<Notification> Update(Guid id, Notification notification)
    {
        var noti = await _unitOfWork.NotificationRepository.GetByIdAsync(id) ?? throw new AppException("Notification not found");
        noti.IsRead = notification.IsRead;
        noti.Link = notification.Link;
        noti.Message = notification.Message;
        await UpdateAsync(noti);

        return new Notification
        {
            Id = id,
            AccountId = noti.AccountId,
            CreatedDate = noti.CreatedDate,
            IsRead = noti.IsRead,
            Link = noti.Link,
            Message = noti.Message,
            NotificationTypeId = noti.NotificationTypeId,
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var noti = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
        if (noti == null)
            throw new AppException("Notification not found");

        return await DeleteById(id);
    }
}