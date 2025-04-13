using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class Permission
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Role> Roles { get; set; }

    public virtual ICollection<PermissionRole> PermissionRole { get; set; }
}