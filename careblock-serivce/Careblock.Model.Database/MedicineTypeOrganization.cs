using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class MedicineTypeOrganization
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(MedicineType))]
    public int MedicineTypeId { get; set; }

    [Required]
    [ForeignKey(nameof(Organization))]
    public Guid OrganizationId { get; set; }

    public virtual MedicineType MedicineType { get; set; }

    public virtual Organization Organization { get; set; }
}
