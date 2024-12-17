using Careblock.Model.Web.Appointment;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IAppointmentDetailService
{
    Task<List<AppointmentDetailDto>> GetAll();
    Task<AppointmentDetailDto> GetById(Guid id); 
    Task<Guid> Create(AppointmentDetailFormDto appointment);
    Task<bool> Update(AppointmentDetailDto appointment);
    Task<bool> Delete(Guid id);
}