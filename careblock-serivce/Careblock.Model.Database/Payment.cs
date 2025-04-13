using Careblock.Model.Shared.Enum;
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

    public PaymentValue Status { get; set; }

    public double Total { get; set; }

    public string? PaidHash { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; }

    public virtual Appointment Appointment { get; set; }
}