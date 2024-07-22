using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class DiagnosticRepository : GenericRepository<Diagnostic>, IDiagnosticRepository
{
    public DiagnosticRepository(IDbContext context) : base(context)
    {
    }
}