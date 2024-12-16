using System.ComponentModel.DataAnnotations;

namespace Careblock.Model.Database;

public class Organization
{
    [Key]
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Address { get; set; }

    public string? MapUrl { get; set; }

    public string? Thumbnail { get; set; }

    public string? Tel { get; set; }

    public string? Fax { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Department> Departments { get; set; }

    public virtual ICollection<Specialist> Specialists { get; set; }

    public virtual ICollection<ExaminationPackage> ExaminationPackages { get; set; }

    public virtual ICollection<ExaminationTypeOrganization> ExaminationTypeOrganizations { get; set; }

    public virtual ICollection<MedicineTypeOrganization> MedicineTypeOrganizations { get; set; }
}