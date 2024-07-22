using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class Diagnostic
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Patient))] 
    public Guid PatientId { get; set; }

    [Required]
    [ForeignKey(nameof(Doctor))] 
    public Guid DoctorId { get; set; }

    public string? Disease { get; set; }

    public DateTime? Time { get; set; }

    public float? Weight { get; set; }

    public float? Height { get; set; }

    public float? HeartRate { get; set; }

    public float? BodyTemperature { get; set; }

    public float? DiastolicBloodPressure { get; set; }

    public float? SystolicBloodPressure { get; set; }

    public string? Note { get; set; }

    [Required]
    public DiagnosticStatus Status { get; set; } // {1: Healthy, 2: Weak, 3: Critical, 4: Normal, 5: Pathological}

    public virtual Account Patient { get; set; }

    public virtual Account Doctor { get; set; }
}