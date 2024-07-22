using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Account;

public class PatientDto
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public BloodType? BloodType { get; set; }

    public string? Phone { get; set; }

    public byte Role { get; set; }

    public string? Avatar { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? AppointmentId { get; set; }
}