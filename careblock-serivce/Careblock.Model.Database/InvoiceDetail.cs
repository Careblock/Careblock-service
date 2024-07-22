using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class InvoiceDetail
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(MedicalService))] 
    public Guid? ServiceId { get; set; }


    [ForeignKey(nameof(Medicine))] 
    public Guid? MedicineId { get; set; }

    [ForeignKey(nameof(Invoice))] 
    public Guid InvoiceId { get; set; }

    public int Quantity { get; set; }

    public virtual MedicalService MedicalService { get; set; }

    public virtual Medicine Medicine { get; set; }

    public virtual Invoice Invoice { get; set; }
}