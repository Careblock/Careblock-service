namespace Careblock.Model.Web.Organization;

public class OrganizationDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? Avatar { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }
}