namespace Careblock.Model.Web.MedicalService;

public class MedicalServiceDto
{
    public Guid Id { get; set; }
    
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Avatar { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }
}