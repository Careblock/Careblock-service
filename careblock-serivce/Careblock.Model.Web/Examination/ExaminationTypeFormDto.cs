using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Examination;

public class ExaminationTypeFormDto
{
    public string Name { get; set; } = null!;

    public IFormFile? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}