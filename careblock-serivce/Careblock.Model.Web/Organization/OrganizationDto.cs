namespace Careblock.Model.Web.Organization;

public class OrganizationDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Address { get; set; }

    public string? MapUrl { get; set; }

    public string? Thumbnail { get; set; }

    public string? Tel { get; set; }

    public string? Fax { get; set; }

    public string? Website { get; set; }
}