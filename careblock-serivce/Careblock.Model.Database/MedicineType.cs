using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class MedicineType
{
    [Key]
    public int Id { get; set; }

    // Tablet, Capsule, Solution, Injection, Powder
    public string Name { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;

    public virtual ICollection<Medicine> Medicines { get; set; }
}