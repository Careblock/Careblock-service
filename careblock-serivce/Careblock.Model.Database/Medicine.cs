using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Careblock.Model.Database;

public class Medicine
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(MedicineType))] 
    public int MedicineTypeId { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public byte UnitPrice { get; set; }

    public string? Description { get; set; }

    public string? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MedicineResult> MedicineResults { get; set; }

    public virtual MedicineType MedicineType { get; set; }
}