using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Specialist
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Organization))] 
    public Guid OrganizationId { get; set; }

    // Skeletal, respiratory, physical therapy...
    public string Name { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }

    public bool IsHidden { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public virtual ICollection<ExaminationOption> ExaminationOption { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ICollection<DoctorSpecialist> DoctorSpecialists { get; set; }

    public virtual ICollection<ExaminationPackageSpecialist> ExaminationPackageSpecialists { get; set; }

}