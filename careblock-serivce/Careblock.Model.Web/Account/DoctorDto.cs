using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;

namespace Careblock.Model.Web.Account;

public class DoctorDto
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = string.Empty;
    
    public string Lastname { get; set; } = string.Empty;
    
    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    
    public string? Phone { get; set; }
    
    public Guid? OrganizationId { get; set; }
    
    public byte? Seniority { get; set; }
    
    public string? Avatar { get; set; }

    public List<ExistedAppointmentDto>? Appointments { get; set; }
}