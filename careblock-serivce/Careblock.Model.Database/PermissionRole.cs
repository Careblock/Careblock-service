using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class PermissionRole
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Permission))]
    public int PermissionId { get; set; }

    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }

    public virtual Permission Permission { get; set; }

    public virtual Role Role { get; set; }
}