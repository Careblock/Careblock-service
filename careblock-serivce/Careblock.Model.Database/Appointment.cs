using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Doctor))] 

    public Guid DoctorId { get; set; }

    [Required]
    [ForeignKey(nameof(Patient))] 
    public Guid PatientId { get; set; }

    [Required] 
    public AppointmentStatus Status { get; set; } // {1: Active, 2: PostPoned, 3: Rejected, 4: CheckedIn}

    public string? Reason { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }

    public virtual Account Doctor { get; set; }

    public virtual Account Patient { get; set; }
}
