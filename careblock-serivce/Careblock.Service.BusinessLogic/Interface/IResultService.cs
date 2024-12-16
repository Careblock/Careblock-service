using Careblock.Model.Database;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IResultService
{
    Task<List<Result>> GetByAppointment(Guid appointmentId); 
    Task<Guid> Create(ResultFormDto result);
    Task<bool> Update(ResultDto result);
    Task<bool> Delete(Guid id);
}