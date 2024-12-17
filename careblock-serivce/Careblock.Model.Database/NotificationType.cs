using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class NotificationType
{
    [Key]
    public int Id { get; set; }

    // info, warning, important
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Notification> Notifications { get; set; }
}