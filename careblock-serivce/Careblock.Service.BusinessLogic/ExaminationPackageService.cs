using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Web.Examination;
using Careblock.Model.Web.TimeSlot;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class ExaminationPackageService : EntityService<ExaminationPackage>, IExaminationPackageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly DatabaseContext _dbContext;

    public ExaminationPackageService(IUnitOfWork unitOfWork, DatabaseContext dbContext, IStorageService storageService) : base(unitOfWork, unitOfWork.ExaminationPackageRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _dbContext = dbContext;
    }

    public async Task<List<ExaminationPackageDto>> GetAll()
    {
        return await _unitOfWork.ExaminationPackageRepository.GetAll().Where(pk => pk.IsDeleted == false).Select(pk => new ExaminationPackageDto
        {
            Id = pk.Id,
            Name = pk.Name,
            OrganizationId = pk.OrganizationId,
            Thumbnail = pk.Thumbnail,
            ExaminationTypeId = pk.ExaminationTypeId,
            ExaminationTypeName = pk.ExaminationType != null ? pk.ExaminationType.Name : string.Empty,
            OrganizationName = pk.Organization.Name,
            ModifiedDate = pk.ModifiedDate,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<ExaminationPackage> GetById(Guid id)
    {
        var examinationPackage = await _unitOfWork.ExaminationPackageRepository.GetByIdAsync(id);
        if (examinationPackage == null || examinationPackage.IsDeleted == true) return new ExaminationPackage();
        return examinationPackage;
    }

    public async Task<List<ExaminationPackageResponseDto>> GetByType(int examinationTypeId)
    {
        try
        {
            var result = _unitOfWork.ExaminationPackageRepository.GetAll()
                .Where(ep => ep.ExaminationTypeId == examinationTypeId && ep.IsDeleted == false)
                .Select(ep => new ExaminationPackageResponseDto
                {
                    Id = ep.Id,
                    OrganizationId = ep.OrganizationId,
                    ExaminationTypeId = ep.ExaminationTypeId,
                    Name = ep.Name,
                    Thumbnail = ep.Thumbnail,
                    OrganizationName = ep.Organization.Name,
                    OrganizationLocation = ep.Organization.Address,
                    ModifiedDate = ep.ModifiedDate,
                    Price = _unitOfWork.ExaminationOptionRepository.GetAll()
                        .Join(_unitOfWork.ExaminationPackageOptionRepository.GetAll(),
                        eo => eo.Id,
                        epo => epo.ExaminationOptionId,
                        (eo, epo) => new { eo, epo })
                        .Where(join => join.epo.ExaminationPackageId == ep.Id)
                        .Sum(join => join.eo.Price),
                    TimeSlots = ep.TimeSlots.Select(x => new TimeSlotDto
                    {
                        Id = x.Id,
                        Period = x.Period,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime
                    }).ToList(),
                    Appointments = ep.Appointments.Select(x => new ExistedAppointmentDto
                    {
                        Id = x.Id,
                        EndDateExpectation = x.EndDateExpectation,
                        StartDateExpectation = x.StartDateExpectation,
                        Status = x.Status
                    }).ToList()
                }).OrderByDescending(x => x.ModifiedDate)
                .ToList();

            return result;
        }
        catch
        {
            return new List<ExaminationPackageResponseDto>();
        }
    }

    public async Task<List<ExaminationPackageResponseDto>> GetByTypeAndOrganization(int examinationTypeId, Guid organizationId)
    {
        try
        {
            var result = _unitOfWork.ExaminationPackageRepository.GetAll()
                .Where(ep => ep.ExaminationTypeId == examinationTypeId && Guid.Equals(ep.OrganizationId, organizationId) && ep.IsDeleted == false)
                .Select(ep => new ExaminationPackageResponseDto
                {
                    Id = ep.Id,
                    OrganizationId = ep.OrganizationId,
                    ExaminationTypeId = ep.ExaminationTypeId,
                    Name = ep.Name,
                    Thumbnail = ep.Thumbnail,
                    OrganizationName = ep.Organization.Name,
                    OrganizationLocation = ep.Organization.Address,
                    ModifiedDate = ep.ModifiedDate,
                    Price = _unitOfWork.ExaminationOptionRepository.GetAll()
                        .Join(_unitOfWork.ExaminationPackageOptionRepository.GetAll(),
                        eo => eo.Id,
                        epo => epo.ExaminationOptionId,
                        (eo, epo) => new { eo, epo })
                        .Where(join => join.epo.ExaminationPackageId == ep.Id)
                        .Sum(join => join.eo.Price),
                    TimeSlots = ep.TimeSlots.Select(x => new TimeSlotDto
                    {
                        Id = x.Id,
                        Period = x.Period,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime
                    }).ToList(),
                    Appointments = ep.Appointments.Select(x => new ExistedAppointmentDto
                    {
                        Id = x.Id,
                        EndDateExpectation = x.EndDateExpectation,
                        StartDateExpectation = x.StartDateExpectation,
                        Status = x.Status
                    }).ToList()
                }).OrderByDescending(x => x.ModifiedDate)
                .ToList();

            return result;
        }
        catch
        {
            return new List<ExaminationPackageResponseDto>();
        }
    }

    public async Task<List<ExaminationPackageResponseDto>> GetByOrganization(Guid userId)
    {
        try
        {
            var organizationId = await _dbContext.Accounts
                .Where(acc => Guid.Equals(acc.Id, userId))
                .Join(_dbContext.Departments,
                 acc => acc.DepartmentId,
                 dep => dep.Id,
                (acc, dep) => dep.OrganizationId)
                .FirstOrDefaultAsync();

            if (Guid.Equals(organizationId, Guid.Empty)) return new List<ExaminationPackageResponseDto>();

            var result = _unitOfWork.ExaminationPackageRepository.GetAll()
                .Where(ep => Guid.Equals(ep.OrganizationId, organizationId) && ep.IsDeleted == false)
                .Select(ep => new ExaminationPackageResponseDto
                {
                    Id = ep.Id,
                    OrganizationId = ep.OrganizationId,
                    ExaminationTypeId = ep.ExaminationTypeId,
                    ExaminationTypeName = ep.ExaminationType != null ? ep.ExaminationType.Name : string.Empty,
                    Name = ep.Name,
                    Thumbnail = ep.Thumbnail,
                    OrganizationName = ep.Organization.Name,
                    OrganizationLocation = ep.Organization.Address,
                    ModifiedDate = ep.ModifiedDate,
                    Price = _unitOfWork.ExaminationOptionRepository.GetAll()
                        .Join(_unitOfWork.ExaminationPackageOptionRepository.GetAll(),
                        eo => eo.Id,
                        epo => epo.ExaminationOptionId,
                        (eo, epo) => new { eo, epo })
                        .Where(join => join.epo.ExaminationPackageId == ep.Id)
                        .Sum(join => join.eo.Price),
                    TimeSlots = ep.TimeSlots.Select(x => new TimeSlotDto
                    {
                        Id = x.Id,
                        Period = x.Period,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime
                    }).ToList(),
                    Appointments = ep.Appointments.Select(x => new ExistedAppointmentDto
                    {
                        Id = x.Id,
                        EndDateExpectation = x.EndDateExpectation,
                        StartDateExpectation = x.StartDateExpectation,
                        Status = x.Status
                    }).ToList()
                }).OrderByDescending(x => x.ModifiedDate)
                .ToList();

            return result;
        }
        catch
        {
            return new List<ExaminationPackageResponseDto>();
        }
    }

    public async Task<Guid> Create(ExaminationPackageFormDto examinationPackage, Guid userId)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) throw new AppException("Organization is not found");

        var newPackage = new ExaminationPackage()
        {
            Id = Guid.NewGuid(),
            Name = examinationPackage.Name,
            Thumbnail = await _storageService.UploadImage(examinationPackage.Thumbnail),
            CreatedDate = DateTime.Now,
            ExaminationTypeId = examinationPackage.ExaminationTypeId,
            IsDeleted = false,
            ModifiedDate = DateTime.Now,
            OrganizationId = examinationPackage.OrganizationId,
        };

        await CreateAsync(newPackage);

        await _dbContext.ExaminationTypeOrganizations.AddAsync(new ExaminationTypeOrganization
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            ExaminationTypeId = examinationPackage.ExaminationTypeId
        });

        await _dbContext.SaveChangesAsync();

        return newPackage.Id;
    }

    public async Task<Guid> CreateNew(ExaminationPackageFormDto examinationPackage)
    {
        var newPackage = new ExaminationPackage()
        {
            Id = Guid.NewGuid(),
            Name = examinationPackage.Name,
            Thumbnail = await _storageService.UploadImage(examinationPackage.Thumbnail),
            CreatedDate = DateTime.Now,
            ExaminationTypeId = examinationPackage.ExaminationTypeId,
            IsDeleted = false,
            ModifiedDate = DateTime.Now,
            OrganizationId = examinationPackage.OrganizationId,
        };

        await CreateAsync(newPackage);

        return newPackage.Id;
    }

    public async Task<ExaminationPackageDto> Update(Guid id, ExaminationPackageFormDto examinationPackage)
    {
        var examPackage = await _unitOfWork.ExaminationPackageRepository.GetByIdAsync(id) ?? throw new AppException("Examination package not found");

        examPackage.Name = examinationPackage.Name;
        examPackage.ModifiedDate = DateTime.Now;
        examPackage.OrganizationId = examinationPackage.OrganizationId;
        examPackage.ExaminationTypeId = examinationPackage.ExaminationTypeId;
        if (examinationPackage.Thumbnail != null) examPackage.Thumbnail = await _storageService.UploadImage(examinationPackage.Thumbnail);

        await UpdateAsync(examPackage);

        return new ExaminationPackageDto
        {
            Id = id,
            Name = examPackage.Name,
            Thumbnail = examPackage.Thumbnail,
            ExaminationTypeId = examPackage.ExaminationTypeId,
            OrganizationId = examinationPackage.OrganizationId,
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var examPackage = await _unitOfWork.ExaminationPackageRepository.GetByIdAsync(id) ?? throw new AppException("Examination package not found");

        examPackage.IsDeleted = true;

        await UpdateAsync(examPackage);

        return true;
    }
}