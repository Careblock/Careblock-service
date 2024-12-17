using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Account;

public class PatientDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Avatar { get; set; }

    public string? Reason { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? ExaminationPackageName { get; set; }

    public string Address { get; set; } = string.Empty;

    public string? Email { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public DateTime? StartDateExpectation { get; set; }

    public Guid? AppointmentId { get; set; }
}