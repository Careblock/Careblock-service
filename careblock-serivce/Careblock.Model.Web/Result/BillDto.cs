using Careblock.Model.Web.Examination;

namespace Careblock.Model.Database;

public class BillDto
{
    public string PatientName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? DoctorName { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string OrganizationName { get; set; } = string.Empty;

    public string ExaminationPackageName { get; set; } = string.Empty;

    public List<ExaminationOptionDto>? ExaminationOptions { get; set; }

    public double TotalPrice { get; set; }

    public DateTime? CreatedDate { get; set; }
}
