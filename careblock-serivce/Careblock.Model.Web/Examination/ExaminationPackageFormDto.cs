using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Examination;

public class ExaminationPackageFormDto
{
    public Guid OrganizationId { get; set; }

    public int ExaminationTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public bool? IsDeleted { get; set; }

    public IFormFile? Thumbnail { get; set; }
}