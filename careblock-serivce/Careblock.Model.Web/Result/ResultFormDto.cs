namespace Careblock.Model.Database;

public class ResultFormDto
{
    public Guid AppointmentId { get; set; }

    // pdf url
    public string? DiagnosticUrl { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }
}
