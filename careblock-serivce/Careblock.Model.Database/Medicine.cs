using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Medicine
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Organization))] 
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Note { get; set; }

    public string? Avatar { get; set; }

    public bool? IsDeleted { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Organization Organization { get; set; }
}