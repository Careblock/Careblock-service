namespace Careblock.Model.Database;

public class ResultFormDto
{

    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }

    // pdf url
    public string? DiagnosticUrl { get; set; }

    public string? Message { get; set; }

    public string? HashName { get; set; }

    public string? SignHash { get; set; }

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }
}
