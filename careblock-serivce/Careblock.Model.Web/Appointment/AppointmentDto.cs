using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Appointment;


public class AppointmentDto
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public AppointmentStatus Status { get; set; } // {1: Active, 2: PostPoned, 3: Rejected, 4: CheckedIn}

    public string? Reason { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}