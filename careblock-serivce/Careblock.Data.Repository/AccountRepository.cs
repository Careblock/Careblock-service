using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class AccountRepository: GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(IDbContext context) : base(context)
    {
    }
}
