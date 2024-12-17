using Microsoft.AspNetCore.Http;

namespace Careblock.Model.Web.Medicine;

public class MedicineFormDto
{
    public int MedicineTypeId { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public byte UnitPrice { get; set; }

    public string? Description { get; set; }

    public IFormFile? Thumbnail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }
}