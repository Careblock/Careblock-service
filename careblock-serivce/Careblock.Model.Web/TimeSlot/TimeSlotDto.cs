namespace Careblock.Model.Web.TimeSlot;

public class TimeSlotDto
{
    public Guid Id { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int Period { get; set; }
}