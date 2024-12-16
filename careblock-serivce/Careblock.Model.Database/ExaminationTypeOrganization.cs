using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationTypeOrganization
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(ExaminationType))]
    public int? ExaminationTypeId { get; set; }

    [Required]
    [ForeignKey(nameof(Organization))]
    public Guid OrganizationId { get; set; }

    public virtual ExaminationType ExaminationType { get; set; }

    public virtual Organization Organization { get; set; }
}
