using Careblock.Model.Database;
using Careblock.Model.Web.Payment;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IPaymentService
{
    Task<List<Payment>> GetAll();
    Task<Payment> GetById(Guid id);
    Task<Payment> GetPaidByAppointmentId(Guid appointmentId);
    Task<Guid> Create(PaymentFormDto payment);
    Task<Payment> Update(Guid id, PaymentFormDto payment);
    Task<Guid> UpdateByAppointment(Guid appointmentId, PaymentFormDto payment);
    Task<bool> Delete(Guid id);
}