using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expires { get; set; }

    public DateTime Created { get; set; }

    public string CreatedByIp { get; set; } = string.Empty;

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReasonRevoked { get; set; }
    
    [NotMapped]
    public bool IsRevoked => Revoked != null;

    [NotMapped]
    public bool IsActive => Revoked == null && DateTime.Now < Expires;

    public Account Account { get; set; } = null!;
}