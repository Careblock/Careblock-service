using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class TimeSlot
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(ExaminationPackage))] 
    public Guid ExaminationPackageId { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    // by minutes
    public int Period { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ExaminationPackage ExaminationPackage { get; set; }
}