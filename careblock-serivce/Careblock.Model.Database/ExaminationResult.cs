using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationResult
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }

    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; } 

    public DateTime? ModifiedDate { get; set; }

    public virtual Account Patient { get; set; }
}
