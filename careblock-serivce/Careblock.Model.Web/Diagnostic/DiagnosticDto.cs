using Careblock.Model.Shared.Enum;

namespace Careblock.Model.Web.Diagnostic
{
    public class DiagnosticDto
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public string? Disease { get; set; }

        public DateTime? Time { get; set; }

        public float? Weight { get; set; }

        public float? Height { get; set; }

        public float? HeartRate { get; set; }

        public float? BodyTemperature { get; set; }

        public float? DiastolicBloodPressure { get; set; }

        public float? SystolicBloodPressure { get; set; }

        public string? Note { get; set; }

        public DiagnosticStatus Status { get; set; } // {1: Healthy, 2: Weak, 3: Critical, 4: Normal, 5: Pathological}
    }
}
