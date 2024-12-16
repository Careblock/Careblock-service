using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Careblock.Model.Database;

public class ExaminationPackage
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Organization))]
    public Guid OrganizationId { get; set; }

    [Required]
    [ForeignKey(nameof(ExaminationType))]
    public int ExaminationTypeId { get; set; }

    // Khám Tổng Quát, Gói Khám Tiền Sản, Gói Khám Tim Mạch, ...
    public string Name { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public virtual ICollection<ExaminationPackageOption> ExaminationPackageOptions { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ExaminationType? ExaminationType { get; set; }

    public virtual ICollection<TimeSlot> TimeSlots { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; }

    public virtual ICollection<ExaminationPackageSpecialist> ExaminationPackageSpecialists { get; set; }
}
