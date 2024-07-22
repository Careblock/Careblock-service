using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Invoice
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Appointment))] 

    public Guid AppointmentId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
    
    public virtual Appointment Appointment { get; set; }
}