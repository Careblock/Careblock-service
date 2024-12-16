using Careblock.Data.Repository.Common.DbContext;
using Careblock.Model.Web.Notification;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Careblock.Service.BusinessLogic.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<Guid, string> userToConnectionMap = new ConcurrentDictionary<Guid, string>();
        private readonly DatabaseContext _dbContext;
        private readonly INotificationService _notificationService;

        public NotificationHub(DatabaseContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public override Task OnConnectedAsync()
        {
            Guid userID = Guid.Empty;
            string connectionId = Context.ConnectionId;

            HttpContext? httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var query = httpContext.Request.Query;
                if (!string.IsNullOrWhiteSpace(query["userID"]))
                {
                    userID = Guid.Parse(query["userID"]!);
                }
            }

            userToConnectionMap.TryAdd(userID, connectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId;
            Guid userID = Guid.Empty;

            HttpContext? httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var query = httpContext.Request.Query;
                if (!string.IsNullOrWhiteSpace(query["userID"]))
                {
                    userID = Guid.Parse(query["userID"]!);
                }
            }

            userToConnectionMap.TryRemove(userID, out connectionId!);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(NotificationDto notification)
        {
            string connectionId;
            if (userToConnectionMap.TryGetValue(Guid.Parse(notification.AccountId.ToString()), out connectionId!))
            {
                Guid newId = await _notificationService.Create(notification);

                if (!Guid.Equals(newId, Guid.Empty))
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification");
            }
        }
    }
}
