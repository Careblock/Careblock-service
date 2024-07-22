using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Database;

public class ExaminationResultFormDto
{
    public Guid PatientId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public IFormFile? ResultFile { get; set; }

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }
}
