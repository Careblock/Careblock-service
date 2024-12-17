using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Medicine;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class MedicineTypeService : EntityService<MedicineType>, IMedicineTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;
    private readonly IStorageService _storageService;

    public MedicineTypeService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.MedicineTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<List<MedicineType>> GetAll()
    {
        return await _unitOfWork.MedicineTypeRepository.GetAll().OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<MedicineType> GetById(Guid id)
    {
        var medicineType = await _unitOfWork.MedicineTypeRepository.GetByIdAsync(id);
        if (medicineType == null) return new MedicineType();
        return medicineType;
    }

    public async Task<int> Create(MedicineTypeFormDto medicineType)
    {
        var types = await _dbContext.MedicineTypes.ToListAsync();
        var lastestId = 1;
        if (types.Count() > 0)
        {
            lastestId = await _dbContext.MedicineTypes.MaxAsync(t => t.Id) + 1;
        }

        var newMedicineType = new MedicineType()
        {
            Id = lastestId,
            Name = medicineType.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };

        await CreateAsync(newMedicineType);

        return newMedicineType.Id;
    }

    public async Task<MedicineTypeDto> Update(int id, MedicineTypeFormDto medicineType)
    {
        var medType = await _unitOfWork.MedicineTypeRepository.GetByIdAsync(id) ?? throw new AppException("Medicine type not found");

        medType.Name = medicineType.Name;
        medType.ModifiedDate = DateTime.Now;

        await UpdateAsync(medType);

        return new MedicineTypeDto
        {
            Id = id,
            Name = medType.Name,
        };
    }

    public async Task<bool> Delete(int id)
    {
        var medicineType = await _dbContext.MedicineTypes.Where(mt => Guid.Equals(mt.Id, id)).FirstAsync();
        return medicineType == null ? throw new AppException("Medicine type not found") : await DeleteById(id);
    }
}