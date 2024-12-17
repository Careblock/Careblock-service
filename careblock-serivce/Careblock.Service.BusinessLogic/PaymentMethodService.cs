using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class PaymentMethodService : EntityService<PaymentMethod>, IPaymentMethodService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public PaymentMethodService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.PaymentMethodRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<PaymentMethod>> GetAll()
    {
        return await _unitOfWork.PaymentMethodRepository.GetAll().ToListAsync();
    }

    public async Task<PaymentMethod> GetById(Guid id)
    {
        var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdAsync(id);
        if (paymentMethod == null) return new PaymentMethod();
        return paymentMethod;
    }

    public async Task<int> Create(PaymentMethodFormDto paymentMethod)
    {
        var types = await _dbContext.PaymentMethods.ToListAsync();
        var lastestId = 1;
        if (types.Count() > 0)
        {
            lastestId = await _dbContext.PaymentMethods.MaxAsync(t => t.Id) + 1;
        }

        var newPaymentMethod = new PaymentMethod()
        {
            Id = lastestId,
            Name = paymentMethod.Name,
        };

        await CreateAsync(newPaymentMethod);

        return newPaymentMethod.Id;
    }

    public async Task<PaymentMethod> Update(int id, PaymentMethodFormDto paymentMethod)
    {
        var result = await _unitOfWork.PaymentMethodRepository.GetByIdAsync(id) ?? throw new AppException("Payment Method not found");

        result.Name = paymentMethod.Name;

        await UpdateAsync(result);

        return new PaymentMethod
        {
            Id = id,
            Name = result.Name,
        };
    }

    public async Task<bool> Delete(int id)
    {
        var paymentMethod = await _dbContext.PaymentMethods.Where(mt => Guid.Equals(mt.Id, id)).FirstAsync();
        return paymentMethod == null ? throw new AppException("Payment Method not found") : await DeleteById(id);
    }
}