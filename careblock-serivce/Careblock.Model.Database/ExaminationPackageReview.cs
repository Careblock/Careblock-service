using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class ExaminationPackageReview
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ExaminationPackageId { get; set; }

    public Guid? ResultId { get; set; }

    public Guid? AppointmentId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public Guid? ParentId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public int Rating { get; set; }

    [Required]
    public string SignHash { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

    public virtual Account User { get; set; } = null!;

    public virtual ExaminationPackage ExaminationPackage { get; set; } = null!;

    public virtual Result? Result { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ExaminationPackageReview? Parent { get; set; }
}

