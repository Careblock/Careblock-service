using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class PermissionRoleRepository : GenericRepository<PermissionRole>, IPermissionRoleRepository
{
    public PermissionRoleRepository(IDbContext context) : base(context)
    {
    }
}
