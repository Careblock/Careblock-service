namespace Careblock.Model.Web.Medicine;

public class MedicineTypeDto
{
    public int Id { get; set; }

    // Tablet, Capsule, Solution, Injection, Powder
    public string Name { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}