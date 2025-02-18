using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Notification
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Account))] 
    public Guid AccountId { get; set; }

    public Guid? OriginId { get; set; }

    [ForeignKey(nameof(NotificationType))]
    public int NotificationTypeId { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Link { get; set; }

    public bool IsRead { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Account Account { get; set; }

    public virtual NotificationType NotificationType { get; set; }
}