using Careblock.Model.Database;
using Careblock.Model.Web.TimeSlot;

namespace Careblock.Service.BusinessLogic.Interface;

public interface ITimeSlotService
{
    Task<List<TimeSlotResponseDto>> GetAll();
    Task<TimeSlot> GetById(Guid id); 
    Task<Guid> Create(TimeSlotFormDto timeSlot);
    Task<TimeSlotResponseDto> Update(Guid id, TimeSlotFormDto timeSlot);
    Task<bool> Delete(Guid id);
}