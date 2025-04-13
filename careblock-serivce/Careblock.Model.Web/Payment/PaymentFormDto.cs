using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Payment;

public class PaymentFormDto
{
    public string? Name { get; set; } = null!;

    public Guid AppointmentId { get; set; }

    public int? PaymentMethodId { get; set; }

    public PaymentValue Status { get; set; }

    public double Total { get; set; }

    public string? PaidHash { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}