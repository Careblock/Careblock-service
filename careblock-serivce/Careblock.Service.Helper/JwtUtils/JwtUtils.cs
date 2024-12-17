using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Careblock.Service.Helper.JwtUtils;

public class JwtUtils : IJwtUtils
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;

    public JwtUtils(
        IUnitOfWork unitOfWork,
        IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(Account account)
    {
        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var roleNames = _unitOfWork.AccountRoleRepository.GetAll().Where(x => Guid.Equals(x.AccountId, account.Id)).Select(x => x).Join(_unitOfWork.RoleRepository.GetAll(), accountRole => accountRole.RoleId, role => role.Id, (accountRole, role) => role.Name).ToList();

        var roles = string.Join(',', string.Join(',', roleNames));
    
        var claims = new List<Claim>
        {
            new Claim("id", account.Id.ToString()),
            new Claim("roles", roles),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var accountId = new Guid(jwtToken.Claims.First(x => x.Type == "id").Value);

            // return account id from JWT token if validation successful
            return accountId;
        }
        catch
        {
            // return Guid.Empty if validation fails
            return Guid.Empty;
        }
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };

        return refreshToken;
    }
}