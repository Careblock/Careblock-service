namespace Careblock.Model.Web.Medicine;

public class MedicineDto
{
    public Guid Id { get; set; }

    public int MedicineTypeId { get; set; }

    public string? MedicineTypeName { get; set; }

    public string? Name { get; set; }

    public float Price { get; set; }

    public int UnitPrice { get; set; }

    public string? Description { get; set; }

    public string? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }
}