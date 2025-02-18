using Careblock.Model.Shared.Enum;
using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Account;

public class AccountFormDto
{
    public Guid? DepartmentId { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string? WalletAddress { get; set; }

    public string? AssetToken { get; set; }

    public string Firstname { get; set; } = string.Empty;
    
    public string Lastname { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public IFormFile? Avatar { get; set; }

    public string? IdentityId { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; } = string.Empty;

    public string? Description { get; set; }

    public byte? Seniority { get; set; }

    public RoleType Role { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool? IsDisable { get; set; } = false;
}