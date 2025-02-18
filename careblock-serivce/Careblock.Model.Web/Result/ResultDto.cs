using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class ResultDto
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    // pdf url
    public string? DiagnosticUrl { get; set; }

    public string? Message { get; set; }

    public string? HashName { get; set; }

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }

    public string? SignHash { get; set; }

    public int Status { get; set; }
}
