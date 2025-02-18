using Careblock.Model.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationPackageOption
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(ExaminationOption))]
    public Guid ExaminationOptionId { get; set; }

    [ForeignKey(nameof(ExaminationPackage))]
    public Guid ExaminationPackageId { get; set; }

    public Priority Priority { get; set; }
    
    public int Sequency { get; set; }

    public virtual ExaminationOption ExaminationOption { get; set; }

    public virtual ExaminationPackage ExaminationPackage { get; set; }
}
