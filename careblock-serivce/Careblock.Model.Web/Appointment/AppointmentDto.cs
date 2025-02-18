using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Appointment;

public class AppointmentDto
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public Guid? OrganizationId { get; set; }

    public Guid? ExaminationPackageId { get; set; }

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

    public DateTime? ModifiedDate { get; set; }
}