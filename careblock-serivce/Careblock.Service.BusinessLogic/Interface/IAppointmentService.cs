using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAll();
    Task<AppointmentDto> GetById(Guid id); 
    Task<List<AppointmentHistoryDto>> GetByPatientID(Guid patientId); 
    Task<bool> UpdateStatus(AppointmentStatus status, Guid id);
    Task<Guid> Create(AppointmentFormDto appointment);
    Task<bool> Update(AppointmentDto appointment);
    Task<bool> Delete(Guid id);
}