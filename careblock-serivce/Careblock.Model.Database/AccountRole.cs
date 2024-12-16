using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class AccountRole
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Account))]
    public Guid? AccountId { get; set; }

    [ForeignKey(nameof(Role))]
    public int? RoleId { get; set; }

    public virtual Account Account { get; set; }

    public virtual Role Role { get; set; }
}