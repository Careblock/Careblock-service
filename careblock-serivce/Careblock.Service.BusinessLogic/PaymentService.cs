using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Payment;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Careblock.Model.Shared.Enum;

namespace Careblock.Service.BusinessLogic;

public class PaymentService : EntityService<Payment>, IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public PaymentService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.PaymentRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<Payment>> GetAll()
    {
        return await _unitOfWork.PaymentRepository.GetAll().ToListAsync();
    }

    public async Task<Payment> GetById(Guid id)
    {
        var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
        if (payment == null) return new Payment();
        return payment;
    }

    public async Task<Payment> GetPaidByAppointmentId(Guid appointmentId)
    {
        var payment = await _unitOfWork.PaymentRepository.GetAll().Where(payment => Guid.Equals(payment.AppointmentId, appointmentId) && payment.Status == PaymentValue.Paid).FirstOrDefaultAsync();
        if (payment == null) return new Payment();
        return payment;
    }

    public async Task<Guid> Create(PaymentFormDto payment)
    {
        var types = await _dbContext.Payments.ToListAsync();

        var newPayment = new Payment()
        {
            Id = new Guid(),
            AppointmentId = payment.AppointmentId,
            PaymentMethodId = payment.PaymentMethodId ?? 0,
            Status = payment.Status,
            Total = payment.Total,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };

        await CreateAsync(newPayment);

        return newPayment.Id;
    }

    public async Task<Payment> Update(Guid id, PaymentFormDto payment)
    {
        var result = await _unitOfWork.PaymentRepository.GetByIdAsync(id) ?? throw new AppException("Payment not found");

        payment.ModifiedDate = DateTime.Now;
        payment.PaymentMethodId = payment.PaymentMethodId;
        payment.Status = payment.Status;

        await UpdateAsync(result);

        return new Payment
        {
            Id = id,
        };
    }

    public async Task<Guid> UpdateByAppointment(Guid appointmentId, PaymentFormDto payment)
    {
        try
        {
            var result = await _unitOfWork.PaymentRepository.GetAll().FirstAsync(x => Guid.Equals(x.AppointmentId, appointmentId)) ?? throw new AppException("Payment not found");
            if (payment.PaymentMethodId == null) {
                throw new AppException("Payment Method not found");
            }

            result.ModifiedDate = DateTime.Now;
            result.PaymentMethodId = payment.PaymentMethodId ?? -1;
            result.Status = payment.Status;
            result.PaidDate = DateTime.Now;
            result.PaidHash = payment.PaidHash;

            await UpdateAsync(result);

            return payment.AppointmentId;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        var payment = await _dbContext.Payments.Where(mt => Guid.Equals(mt.Id, id)).FirstAsync();
        return payment == null ? throw new AppException("Payment not found") : await DeleteById(id);
    }
}