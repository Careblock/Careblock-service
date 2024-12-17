namespace Careblock.Model.Web.TimeSlot;

public class TimeSlotResponseDto
{
    public Guid Id { get; set; }

    public Guid ExaminationPackageId { get; set; }

    public string? ExaminationPackageName { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    // by minutes
    public int Period { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}