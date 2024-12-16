using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Data.Repository;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IDbContext context) : base(context)
    {
    }
    
    public async Task<List<RefreshToken>> GetRefreshTokens(Guid accountId)
    {
        return await DbSet.Where(r => r.AccountId == accountId).ToListAsync();
    }

    public async Task<RefreshToken?> GetToken(string token)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Token == token);
    }
}