using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Medicine;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class MedicineService : EntityService<Medicine>, IMedicineService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly DatabaseContext _dbContext;

    public MedicineService(IUnitOfWork unitOfWork, DatabaseContext dbContext, IStorageService storageService) : base(unitOfWork, unitOfWork.MedicineRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _dbContext = dbContext;
    }

    public async Task<List<MedicineDto>> GetAll()
    {
        return await _unitOfWork.MedicineRepository.GetAll().Where(med => med.IsDeleted == false).Select(x => new MedicineDto
        {
            CreatedDate = x.CreatedDate,
            Description = x.Description,
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            MedicineTypeId = x.MedicineTypeId,
            MedicineTypeName = x.MedicineType.Name,
            Name = x.Name,
            ModifiedDate = x.ModifiedDate,
            Price = (float) x.Price,
            Thumbnail = x.Thumbnail,
            UnitPrice = x.UnitPrice,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<Medicine> GetById(Guid id)
    {
        var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(id);
        if (medicine == null || medicine.IsDeleted == true) return new Medicine();
        return medicine;
    }

    public async Task<List<MedicineResponseDto>> GetByType(int medicineTypeId)
    {
        try
        {
            var medicines = await _dbContext.Medicines.Where(x => x.MedicineTypeId == medicineTypeId && x.IsDeleted == false).Select(me => new MedicineResponseDto
            {
                Id = me.Id,
                MedicineTypeId = me.MedicineTypeId,
                Name = me.Name,
                Price = me.Price,
                UnitPrice = me.UnitPrice,
                Description = me.Description,
                Thumbnail = me.Thumbnail,
                ModifiedDate = me.ModifiedDate,
            }).Distinct().ToListAsync();

            return medicines;
        }
        catch (Exception)
        {
            return new List<MedicineResponseDto>();
        }
    }

    public async Task<List<MedicineResponseDto>> GetByTypeAndOrganization(int medicineTypeId, Guid organizationId)
    {
        try
        {
            var medicines = await _dbContext.Medicines
                .Join(_dbContext.MedicineTypes,
                      me => me.MedicineTypeId,
                      mt => mt.Id,
                      (me, mt) => new { me, mt })
                .Join(_dbContext.MedicineTypeOrganizations,
                      combined => combined.mt.Id,
                      mto => mto.MedicineTypeId,
                      (combined, mto) => new { combined.me, combined.mt, mto })
                .Where(result => Guid.Equals(result.mto.OrganizationId, organizationId) && result.me.MedicineTypeId == medicineTypeId && result.me.IsDeleted == false)
                .Select(result => new MedicineResponseDto
                {
                    Id = result.me.Id,
                    Description = result.me.Description,
                    MedicineTypeId = result.me.MedicineTypeId,
                    Name = result.me.Name,
                    Price = result.me.Price,
                    Thumbnail = result.me.Thumbnail,
                    UnitPrice = result.me.UnitPrice,
                })
                .Distinct()
                .ToListAsync();

            return medicines;
        }
        catch (Exception)
        {
            return new List<MedicineResponseDto>();
        }
    }

    public async Task<List<MedicineResponseDto>> GetByOrganization(Guid userId)
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

            if (Guid.Equals(organizationId, Guid.Empty)) throw new AppException("Organization is not found");

            var medicines = await _dbContext.Medicines
                .Join(_dbContext.MedicineTypes,
                      me => me.MedicineTypeId,
                      mt => mt.Id,
                      (me, mt) => new { me, mt })
                .Join(_dbContext.MedicineTypeOrganizations,
                      combined => combined.mt.Id,
                      mto => mto.MedicineTypeId,
                      (combined, mto) => new { combined.me, combined.mt, mto })
                .Where(result => Guid.Equals(result.mto.OrganizationId, organizationId) && result.me.IsDeleted == false)
                .Select(result => new MedicineResponseDto
                {
                    Id = result.me.Id,
                    Description = result.me.Description,
                    MedicineTypeId = result.me.MedicineTypeId,
                    Name = result.me.Name,
                    Price = result.me.Price,
                    Thumbnail = result.me.Thumbnail,
                    UnitPrice = result.me.UnitPrice,
                    ModifiedDate = result.me.ModifiedDate,
                })
                .Distinct()
                .OrderByDescending(x => x.ModifiedDate)
                .ToListAsync();

            return medicines;
        }
        catch (Exception)
        {
            return new List<MedicineResponseDto>();
        }
    }

    public async Task<Guid> Create(MedicineFormDto medicine, Guid userId)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) throw new AppException("Organization is not found");

        var newMedicine = new Medicine()
        {
            Id = Guid.NewGuid(),
            Name = medicine.Name,
            Description = medicine.Description,
            MedicineTypeId = medicine.MedicineTypeId,
            Price = medicine.Price,
            UnitPrice = medicine.UnitPrice,
            Thumbnail = await _storageService.UploadImage(medicine.Thumbnail),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            IsDeleted = false,
        };

        await CreateAsync(newMedicine);

        await _dbContext.MedicineTypeOrganizations.AddAsync(new MedicineTypeOrganization
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            MedicineTypeId = medicine.MedicineTypeId
        });

        await _dbContext.SaveChangesAsync();

        return newMedicine.Id;
    }

    public async Task<Guid> CreateNew(MedicineFormDto medicine)
    {
        var newMedicine = new Medicine()
        {
            Id = Guid.NewGuid(),
            MedicineTypeId = medicine.MedicineTypeId,
            Name = medicine.Name,
            Price = medicine.Price,
            UnitPrice = medicine.UnitPrice,
            Description = medicine.Description,
            Thumbnail = await _storageService.UploadImage(medicine.Thumbnail),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            IsDeleted = false,
        };

        await CreateAsync(newMedicine);

        return newMedicine.Id;
    }

    public async Task<MedicineDto> Update(Guid id, MedicineFormDto medicine)
    {
        var medicineResult = await _unitOfWork.MedicineRepository.GetByIdAsync(id) ?? throw new AppException("Medicine not found");

        medicineResult.Name = medicine.Name;
        medicineResult.UnitPrice = medicine.UnitPrice;
        medicineResult.Price = medicine.Price;
        medicineResult.ModifiedDate = DateTime.Now;
        medicineResult.Description = medicine.Description;
        medicineResult.MedicineTypeId = medicine.MedicineTypeId;
        if (medicine.Thumbnail != null) medicineResult.Thumbnail = await _storageService.UploadImage(medicine.Thumbnail);

        await UpdateAsync(medicineResult);

        return new MedicineDto
        {
            Id = id,
            Name = medicineResult.Name,
            Thumbnail = medicineResult.Thumbnail,
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var medicineResult = await _unitOfWork.MedicineRepository.GetByIdAsync(id) ?? throw new AppException("Medicine not found");

        medicineResult.IsDeleted = true;

        await UpdateAsync(medicineResult);

        return true;
    }
}