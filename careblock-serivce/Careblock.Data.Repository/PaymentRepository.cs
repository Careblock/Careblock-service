using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(IDbContext context) : base(context)
    {
    }
}