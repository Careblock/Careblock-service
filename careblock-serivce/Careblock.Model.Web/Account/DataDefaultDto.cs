using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Account;

public class DataDefaultDto
{
    public Guid? Id { get; set; }

    public string? OrganizationThumbnail { get; set; }

    public string OrganizationName { get; set; } = string.Empty;

    public string? OrganizationAddress { get; set; }

    public string? OrganizationTel { get; set; }

    public string? OrganizationFax { get; set; }

    public string? OrganizationUrl { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public string? Address { get; set; }

    public Guid? PatientId { get; set; }

    public Guid? DoctorId { get; set; }

    public string? DoctorName { get; set; }

    public DateTime? CreatedDate { get; set; }
}