using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class DoctorSpecialist
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }

    [Required]
    [ForeignKey(nameof(Specialist))]
    public Guid SpecialistId { get; set; }

    public DateTime? StartDate { get; set; }

    public virtual Account Account { get; set; }

    public virtual Specialist Specialist { get; set; }
}