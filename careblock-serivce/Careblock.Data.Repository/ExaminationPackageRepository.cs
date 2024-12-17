using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class ExaminationPackageRepository : GenericRepository<ExaminationPackage>, IExaminationPackageRepository
{
    public ExaminationPackageRepository(IDbContext context) : base(context)
    {
    }
}