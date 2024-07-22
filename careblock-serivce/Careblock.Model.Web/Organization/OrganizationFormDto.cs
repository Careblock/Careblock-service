using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Organization;

public class OrganizationFormDto
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public IFormFile? Avatar { get; set; }

    public string? Description { get; set; }
}