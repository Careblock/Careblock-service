using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Result
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Appointment))]
    public Guid AppointmentId { get; set; }

    // pdf url
    public string? DiagnosticUrl { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Appointment Appointment { get; set; }

    public virtual ICollection<MedicineResult> MedicineResults { get; set; }
}