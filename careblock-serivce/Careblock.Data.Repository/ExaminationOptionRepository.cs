using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class ExaminationOptionRepository : GenericRepository<ExaminationOption>, IExaminationOptionRepository
{
    public ExaminationOptionRepository(IDbContext context) : base(context)
    {
    }
}