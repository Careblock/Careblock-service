using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Payment
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Appointment))]
    public Guid AppointmentId { get; set; }

    [ForeignKey(nameof(PaymentMethod))]
    public int PaymentMethodId { get; set; }

    public Payment Status { get; set; }

    public float Total { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; }

    public virtual Appointment Appointment { get; set; }
}