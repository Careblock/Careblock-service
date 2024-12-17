using Careblock.Data.Repository.Common.BaseRepository;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;

namespace Careblock.Data.Repository;

public class ExaminationTypeRepository : GenericRepository<ExaminationType>, IExaminationTypeRepository
{
    public ExaminationTypeRepository(IDbContext context) : base(context)
    {
    }
}