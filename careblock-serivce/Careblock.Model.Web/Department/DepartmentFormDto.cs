namespace Careblock.Model.Web.Department;

public class DepartmentFormDto
{
    public Guid OrganizationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}