using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Model.Database;

namespace Careblock.Data.Repository.Interface;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<List<RefreshToken>> GetRefreshTokens(Guid accountId);

    Task<RefreshToken?> GetToken(string token);
}