using System.Text.Json.Serialization;
using Careblock.Model.Web.Organization;

namespace Careblock.Model.Web.Account;

public class AccountDto
{
    public Guid Id { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public byte Gender { get; set; }
    
    public string? IdentityId { get; set; }
    
    public byte? BloodType { get; set; }
    
    public string? Phone { get; set; }
    
    public byte Role { get; set; }
    
    public string? Avatar { get; set; }
    
    public bool? IsDeleted { get; set; }
    
    public OrganizationDto? Organization { get; set; }
    
    public string JwtToken { get; set; } = string.Empty;

    // other fields...

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; } = string.Empty;
}