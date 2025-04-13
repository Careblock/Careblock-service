using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class Role
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Permission> Permissions { get; set; }

    public virtual ICollection<AccountRole> AccountRoles { get; set; }

    public virtual ICollection<PermissionRole> PermissionRole { get; set; }
}