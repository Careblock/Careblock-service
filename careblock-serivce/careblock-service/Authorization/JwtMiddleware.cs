using Careblock.Data.Repository.Common.DbContext;
using Careblock.Service.Helper.JwtUtils;

namespace careblock_service.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, DatabaseContext dataContext, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            var accountId = jwtUtils.ValidateJwtToken(token);
            if (accountId != Guid.Empty)
            {
                // attach account to context on successful jwt validation
                context.Items["Account"] = await dataContext.Accounts.FindAsync(accountId);
            }
        }
        await _next(context);
    }
}