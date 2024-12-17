using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class ExaminationType
{
    [Key]
    public int Id { get; set; }

    // General, Specialization
    public string Name { get; set; } = string.Empty;

    public string Thumbnail { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public virtual ICollection<ExaminationPackage> ExaminationPackages { get; set; }
}
