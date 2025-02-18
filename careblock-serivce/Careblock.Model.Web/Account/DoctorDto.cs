using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;

namespace Careblock.Model.Web.Account;

public class DoctorDto
{
    public Guid Id { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public string? Avatar { get; set; }

    public string? IdentityId { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public byte? Seniority { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool? IsDisable { get; set; } = false;

    public List<ExistedAppointmentDto>? Appointments { get; set; }

    public List<string>? Roles { get; set; }

    public List<Guid>? Specialist { get; set; } = null;
}