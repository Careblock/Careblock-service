using Careblock.Model.Database;
using Careblock.Model.Shared.Enum;
using System.Text.Json.Serialization;

namespace Careblock.Model.Web.Appointment;

public class AppointmentHistoryDto
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public Guid? OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public Guid? ExaminationPackageId { get; set; }

    public string? ExaminationPackageName { get; set; }

    public int? ExaminationTypeId { get; set; }

    public string? DoctorName { get; set; }

    public string? DoctorAvatar { get; set; }

    public Guid PatientId { get; set; }

    public string? PatientWalletAddress { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public Gender? Gender { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? Symptom { get; set; }

    public AppointmentStatus Status { get; set; } // {1: Active, 2: PostPoned, 3: Rejected, 4: CheckedIn}

    public string? Reason { get; set; }

    public string? Note { get; set; }

    public DateTime? StartDateReality { get; set; }

    public DateTime? EndDateReality { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? StartDateExpectation { get; set; }

    public DateTime? EndDateExpectation { get; set; }


    public List<ResultDto>? Results { get; set; }

    public string? Feedback { get; set; }

    public int? Rating { get; set; }
}