using Careblock.Model.Database;

namespace Careblock.Service.Helper.JwtUtils;

public interface  IJwtUtils
{
    public string GenerateJwtToken(Account account);
    public Guid ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}