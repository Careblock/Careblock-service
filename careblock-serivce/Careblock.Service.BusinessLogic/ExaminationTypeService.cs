using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class ExaminationTypeService : EntityService<ExaminationType>, IExaminationTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;
    private readonly IStorageService _storageService;

    public ExaminationTypeService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.ExaminationTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<List<ExaminationType>> GetAll()
    {
        return await _unitOfWork.ExaminationTypeRepository.GetAll().OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<ExaminationType> GetById(Guid id)
    {
        var examinationType = await _unitOfWork.ExaminationTypeRepository.GetByIdAsync(id);
        if (examinationType == null) return new ExaminationType();
        return examinationType;
    }

    public async Task<List<ExaminationType>> GetByUserId(Guid userId)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) return new List<ExaminationType>();

        var examinationTypes = await _dbContext.ExaminationTypeOrganizations
            .Where(eto => Guid.Equals(eto.OrganizationId, organizationId))
            .Join(_dbContext.Organizations,
                  eto => eto.OrganizationId,
                  org => org.Id,
                  (eto, org) => new { eto, org })
            .Join(_dbContext.ExaminationTypes,
                  combined => combined.eto.ExaminationTypeId,
                  et => et.Id,
                  (combined, et) => et)
            .ToListAsync();

        if (examinationTypes == null) return new List<ExaminationType>();

        return examinationTypes;
    }

    public async Task<int> Create(ExaminationTypeFormDto examinationType)
    {
        var newExaminationType = new ExaminationType()
        {
            Name = examinationType.Name,
            Thumbnail = await _storageService.UploadImage(examinationType.Thumbnail),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };

        await CreateAsync(newExaminationType);

        return newExaminationType.Id;
    }

    public async Task<ExaminationTypeDto> Update(int id, ExaminationTypeFormDto examinationType)
    {
        var examType = await _unitOfWork.ExaminationTypeRepository.GetByIdAsync(id) ?? throw new AppException("Examination type not found");

        examType.Name = examinationType.Name;
        examType.ModifiedDate = DateTime.Now;
        if (examinationType.Thumbnail != null) examType.Thumbnail = await _storageService.UploadImage(examinationType.Thumbnail);

        await UpdateAsync(examType);

        return new ExaminationTypeDto
        {
            Name = examType.Name,
            Thumbnail = examType.Thumbnail,
        };
    }

    public async Task<bool> Delete(int id)
    {
        var examinationType = await _dbContext.ExaminationTypes.Where(sp => Guid.Equals(sp.Id, id)).FirstAsync();
        return examinationType == null ? throw new AppException("Examination type not found") : await DeleteById(id);
    }
}