using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }

    [ForeignKey(nameof(Doctor))] 
    public Guid? DoctorId { get; set; }

    [ForeignKey(nameof(Organization))]
    public Guid? OrganizationId { get; set; }

    [ForeignKey(nameof(ExaminationPackage))]
    public Guid? ExaminationPackageId { get; set; }

    [Required] 
    public AppointmentStatus Status { get; set; } // {1: Active, 2: PostPoned, 3: Rejected, 4: CheckedIn}

    public string? Name { get; set; } = string.Empty;
    
    public Gender? Gender { get; set; }
    
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Symptom { get; set; }

    public string? Note { get; set; }

    public string? Reason { get; set; }

    public DateTime? StartDateExpectation { get; set; }

    public DateTime? EndDateExpectation { get; set; }

    public DateTime? StartDateReality { get; set; }

    public DateTime? EndDateReality { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public virtual Account? Doctor { get; set; }

    public virtual Account Patient { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ExaminationPackage ExaminationPackage { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }

    public virtual ICollection<Payment> Payments { get; set; }

    public virtual Result Result { get; set; }
}
