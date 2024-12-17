namespace Careblock.Model.Web.Examination;

public class ExaminationPackageDto
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public int ExaminationTypeId { get; set; }

    public string? ExaminationTypeName { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public string? Thumbnail { get; set; }
}