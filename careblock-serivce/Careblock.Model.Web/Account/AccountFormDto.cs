using careblock_service.Authorization;
using Careblock.Model.Shared.Enum;
using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Account;

public class AccountFormDto
{
    public string StakeId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;
    
    public string Lastname { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    
    public string? IdentityId { get; set; }
    
    public BloodType? BloodType { get; set; }
    
    public string? Phone { get; set; }
    
    [AllowedRoles(ErrorMessage = "Invalid role. Allowed roles are 'patient' or 'doctor'")]
    public Role Role { get; set; }
    
    public Guid? OrganizationId { get; set; }
    
    public byte? Seniority { get; set; }
    
    public IFormFile? Avatar { get; set; }
}