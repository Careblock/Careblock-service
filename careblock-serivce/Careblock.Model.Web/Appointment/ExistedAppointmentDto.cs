using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Appointment;

public class ExistedAppointmentDto
{
    public Guid Id { get; set; }

    public AppointmentStatus Status { get; set; } // {1: Active, 2: PostPoned, 3: Rejected, 4: CheckedIn}

    public DateTime? StartDateExpectation { get; set; }

    public DateTime? EndDateExpectation { get; set; }
}