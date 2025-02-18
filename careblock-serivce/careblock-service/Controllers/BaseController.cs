using Careblock.Model.Database;
using Microsoft.AspNetCore.Mvc;

namespace careblock_service.Controllers;

[Controller]
public abstract class BaseController : ControllerBase
{
    // returns the current authenticated account (null if not logged in)
    protected Account Account => (Account)HttpContext.Items["Account"]!;
    
    protected Guid CurrentAccountId => Account.Id;
}