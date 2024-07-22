using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class Organization
{
    [Key]
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? Avatar { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}