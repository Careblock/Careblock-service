using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationOption
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Specialist))]
    public Guid SpecialistId { get; set; }

    // Blood tests, Urine tests, X-ray, Gastroscopy, Colonoscopy...
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public double? Price { get; set; }

    // by minutes
    public int? TimeEstimation { get; set; }

    // Lưu cấu trúc field input trên FE dưới dạng Json
    public string? ExaminationForm { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }

    public virtual ICollection<ExaminationPackageOption> ExaminationPackageOptions { get; set; }

    public virtual Specialist Specialist { get; set; }
}
