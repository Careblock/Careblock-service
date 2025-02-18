using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationPackageSpecialist
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Specialist))]
    public Guid? SpecialistId { get; set; }

    [ForeignKey(nameof(ExaminationPackage))]
    public Guid? ExaminationPackageId { get; set; }

    public virtual Specialist Specialist { get; set; }

    public virtual ExaminationPackage ExaminationPackage { get; set; }
}