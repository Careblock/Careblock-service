using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.MedicalService;


public class MedicalServiceFormDto
{
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public IFormFile? Avatar { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }
}