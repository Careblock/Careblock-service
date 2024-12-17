namespace Careblock.Model.Web.Department;

public class DepartmentDto
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}