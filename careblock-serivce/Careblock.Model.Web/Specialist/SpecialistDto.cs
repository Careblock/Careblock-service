namespace Careblock.Model.Web.Specialist;

public class SpecialistDto
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    // Skeletal, respiratory, physical therapy...
    public string Name { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }

    public bool IsHidden { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}