using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Department
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Organization))]
    public Guid OrganizationId { get; set; }

    //Department of bones and joints...
    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public virtual ICollection<Account> Accounts { get; set; }

    public virtual Organization Organization { get; set; }
}