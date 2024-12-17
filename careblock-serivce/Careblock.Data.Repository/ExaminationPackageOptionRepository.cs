using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class ExaminationPackageOptionRepository : GenericRepository<ExaminationPackageOption>, IIExaminationPackageOptionRepository
{
    public ExaminationPackageOptionRepository(IDbContext context) : base(context)
    {
    }
}