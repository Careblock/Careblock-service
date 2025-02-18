using Careblock.Model.Database;
using Careblock.Model.Web.Examination;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IPaymentMethodService
{
    Task<List<PaymentMethod>> GetAll();
    Task<PaymentMethod> GetById(Guid id); 
    Task<int> Create(PaymentMethodFormDto paymentMethod);
    Task<PaymentMethod> Update(int id, PaymentMethodFormDto paymentMethod);
    Task<bool> Delete(int id);
}