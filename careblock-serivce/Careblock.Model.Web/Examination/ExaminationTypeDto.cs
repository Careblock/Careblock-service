namespace Careblock.Model.Web.Examination;

public class ExaminationTypeDto
{
    public string Name { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}