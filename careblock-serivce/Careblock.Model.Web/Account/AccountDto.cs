using System.Text.Json.Serialization;
using Careblock.Model.Web.Department;

namespace Careblock.Model.Web.Account;

public class AccountDto
{
    public Guid Id { get; set; }

    public DepartmentDto? Department { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string? WalletAddress { get; set; }

    public string? AssetToken { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public byte Gender { get; set; }

    public string? Avatar { get; set; }

    public string? IdentityId { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; } = string.Empty;

    public string? Description { get; set; }

    public byte? Seniority { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool? IsDisable { get; set; } = false;

    public string JwtToken { get; set; } = string.Empty;

    public List<string>? Roles { get; set; }

    // other fields...

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; } = string.Empty;
}