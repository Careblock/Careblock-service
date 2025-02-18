namespace Careblock.Model.Web.Appointment;

public class AppointmentRequest
{
    public int PageIndex { get; set; }

    public int PageNumber { get; set; }

    public Guid UserId { get; set; }

    public Guid? DoctorId { get; set; } = Guid.Empty;

    public DateTime? CreatedDate { get; set; }

    public int? ExaminationTypeId { get; set; }

    public string Keyword { get; set; } = string.Empty;
}