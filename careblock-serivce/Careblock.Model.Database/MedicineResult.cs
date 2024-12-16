using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class MedicineResult
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Medicine))]
    public Guid MedicineId { get; set; }

    [ForeignKey(nameof(Result))]
    public Guid ResultId { get; set; }
    
    public int Quantity { get; set; }

    public float Price { get; set; }

    public virtual Result Result { get; set; }

    public virtual Medicine Medicine { get; set; }
}