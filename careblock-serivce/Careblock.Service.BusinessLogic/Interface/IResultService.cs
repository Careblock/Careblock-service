using Careblock.Model.Database;
using Careblock.Model.Web.Result;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IResultService
{
    Task<List<Result>> GetByAppointment(Guid appointmentId); 
    Task<BillDto> GetBill(Guid appointmentId); 
    Task<Guid> Create(ResultFormDto result);
    Task<bool> Update(ResultDto result);
    Task<bool> Delete(Guid id);
    Task<bool> Sign(SignResultInputDto input);
    Task<bool> Send(Guid id);
}