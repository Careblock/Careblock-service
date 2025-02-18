namespace Careblock.Model.Web.Appointment;

public class AppointmentDetailDto
{
    public Guid Id { get; set; }

    public Guid ExaminationOptionId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid DoctorId { get; set; }

    // json - lưu cả cục dữ liệu
    public string Diagnostic { get; set; } = string.Empty;

    public double? Price { get; set; }
}