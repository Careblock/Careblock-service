using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class Account
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Department))]
    public Guid? DepartmentId { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string? Lastname { get; set; }

    public DateTime? DateOfBirth { get; set; }
    
    public Gender? Gender { get; set; }

    public string? Avatar { get; set; }
    
    public string? IdentityId { get; set; }
    
    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }
    
    public byte? Seniority { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool? IsDisable { get; set; } = false;

    public virtual Department? Department { get; set; }

    public virtual ICollection<AccountRole> AccountRoles { get; set; }

    public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }

    [NotMapped]
    public List<RefreshToken> RefreshTokens { get; set; }

    [NotMapped]
    public virtual ICollection<Appointment> Appointments { get; set; }

    public virtual ICollection<DoctorSpecialist> DoctorSpecialists { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }

    public bool OwnsToken(string token) 
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }

    public ICollection<string> GetRoleNames()
    {
        var roleNames = this.AccountRoles?.Select(ar => ar.Role.Name).ToList() ?? new List<string>();
        return roleNames;
    }
}