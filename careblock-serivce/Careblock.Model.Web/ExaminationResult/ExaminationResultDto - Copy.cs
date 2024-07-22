using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Database;

public class ExaminationResultDto
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }

    public IFormFile? ResultFile { get; set; }
}
