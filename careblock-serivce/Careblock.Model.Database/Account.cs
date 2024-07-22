using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Database;

public class Account
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Organization))]
    public Guid? OrganizationId { get; set; }

    public string StakeId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }
    
    public Gender Gender { get; set; }
    
    public string? IdentityId { get; set; }
    
    public string? Phone { get; set; }

    public string? Avatar { get; set; }
    
    public BloodType? BloodType { get; set; }

    public Role Role { get; set; }

    public byte? Seniority { get; set; }
    
    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; } = false;

    public virtual Organization? Organization { get; set; }
    
    [NotMapped]
    public List<RefreshToken> RefreshTokens { get; set; }

    public bool OwnsToken(string token) 
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
}