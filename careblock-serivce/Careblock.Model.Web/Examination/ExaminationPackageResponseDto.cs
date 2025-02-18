using Careblock.Model.Web.Appointment;
using Careblock.Model.Web.TimeSlot;

namespace Careblock.Model.Web.Examination;

public class ExaminationPackageResponseDto
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public int ExaminationTypeId { get; set; }

    public string? ExaminationTypeName { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public double? Price { get; set; }

    public string OrganizationName { get; set; } = string.Empty;

    public string? OrganizationLocation { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public List<TimeSlotDto>? TimeSlots { get; set; }

    public List<ExistedAppointmentDto>? Appointments { get; set; }
}