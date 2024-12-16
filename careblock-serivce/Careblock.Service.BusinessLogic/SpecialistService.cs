using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Specialist;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class SpecialistService : EntityService<Specialist>, ISpecialistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;
    private readonly IStorageService _storageService;

    public SpecialistService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.SpecialistRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<List<SpecialistDto>> GetAll()
    {
        return await _unitOfWork.SpecialistRepository.GetAll().Where(sp => sp.IsHidden == false).Select(sp => new SpecialistDto
        {
            Id = sp.Id,
            Name = sp.Name,
            CreatedDate = sp.CreatedDate,
            Description = sp.Description,
            IsHidden = sp.IsHidden,
            ModifiedDate = sp.ModifiedDate,
            OrganizationId = sp.OrganizationId,
            Thumbnail = sp.Thumbnail,
            OrganizationName = sp.Organization.Name,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<Specialist> GetById(Guid id)
    {
        var specialist = await _unitOfWork.SpecialistRepository.GetByIdAsync(id);
        if (specialist == null || specialist.IsHidden == true) return new Specialist();
        return specialist;
    }
    
    public async Task<List<Specialist>> GetByUserId(Guid userId)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) return new List<Specialist>();

        var specialist = await _unitOfWork.SpecialistRepository.GetAll().Where(sp => Guid.Equals(sp.OrganizationId, organizationId) && sp.IsHidden == false).Select(rs => new Specialist
        {
            Id = rs.Id,
            Name = rs.Name,
            Description = rs.Description,
            IsHidden = rs.IsHidden,
            OrganizationId = organizationId,
            Thumbnail = rs.Thumbnail,
            ModifiedDate = rs.ModifiedDate,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();

        if (specialist == null) return new List<Specialist>();

        return specialist;
    }

    public async Task<bool> AssignSpecialist(Guid userId, SpecialistRequest request)
    {
        try
        {
            var doctorSpecialist = await _dbContext.DoctorSpecialists.Where(ds => Guid.Equals(ds.AccountId, userId)).ToListAsync();
            if (doctorSpecialist.Any())
            {
                _dbContext.DoctorSpecialists.RemoveRange(doctorSpecialist);
                await _dbContext.SaveChangesAsync();
            }

            var newDoctorSpecialist = request.Specialist.Select(sp => new DoctorSpecialist
            {
                Id = Guid.NewGuid(),
                AccountId = userId,
                SpecialistId = sp,
            }).ToList();

            await _dbContext.DoctorSpecialists.AddRangeAsync(newDoctorSpecialist);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Guid> Create(SpecialistFormDto specialist)
    {
        var newSpecialist = new Specialist()
        {
            Id = Guid.NewGuid(),
            Name = specialist.Name,
            Thumbnail = await _storageService.UploadImage(specialist.Thumbnail),
            Description = specialist.Description,
            IsHidden = false,
            OrganizationId = specialist.OrganizationId,
            ModifiedDate = DateTime.Now,
            CreatedDate = DateTime.Now,
        };

        await CreateAsync(newSpecialist);
        return newSpecialist.Id;
    }

    public async Task<SpecialistDto> Update(Guid id, SpecialistFormDto specialist)
    {
        var specialistData = await _unitOfWork.SpecialistRepository.GetByIdAsync(specialist.Id) ?? throw new AppException("Specialist not found");

        specialistData.Name = specialist.Name;
        if (specialist.Thumbnail != null) specialistData.Thumbnail = await _storageService.UploadImage(specialist.Thumbnail);
        specialistData.Description = specialist.Description;
        specialistData.ModifiedDate = DateTime.Now;
        specialistData.OrganizationId = specialist.OrganizationId;

        await UpdateAsync(specialistData);

        return new SpecialistDto
        {
            Id = id,
            Description = specialistData.Description,
            Thumbnail = specialistData.Thumbnail,
            OrganizationId = specialistData.OrganizationId,
            IsHidden = specialistData.IsHidden,
            Name = specialistData.Name
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var specialistData = await _unitOfWork.SpecialistRepository.GetByIdAsync(id) ?? throw new AppException("Specialist not found");

        specialistData.IsHidden = true;

        await UpdateAsync(specialistData);

        return true;
    }
}