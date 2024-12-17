using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class PaymentMethod
{
    [Key]
    public int Id { get; set; }

    // ADA, Visa, Paypal, VNpay...
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Payment> Payments { get; set; }
}