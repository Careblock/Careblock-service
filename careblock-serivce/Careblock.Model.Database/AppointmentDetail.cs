using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class AppointmentDetail
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(ExaminationOption))]
    public Guid ExaminationOptionId { get; set; }

    [Required]
    [ForeignKey(nameof(Appointment))]
    public Guid AppointmentId { get; set; }

    [Required]
    [ForeignKey(nameof(Doctor))] 
    public Guid DoctorId { get; set; }

    // json - lưu cả cục dữ liệu
    public string Diagnostic { get; set; } = string.Empty;
    
    public double? Price { get; set; }

    public virtual Account Doctor { get; set; }

    public virtual ExaminationOption ExaminationOption { get; set; }

    public virtual Appointment Appointment { get; set; }
}
