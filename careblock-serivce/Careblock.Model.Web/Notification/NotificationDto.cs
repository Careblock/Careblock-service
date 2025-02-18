namespace Careblock.Model.Web.Notification;

public class NotificationDto
{
    public Guid? Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid? OriginId { get; set; }

    public int NotificationTypeId { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Link { get; set; }

    public bool IsRead { get; set; }

    public DateTime? CreatedDate { get; set; }
}