using Careblock.Model.Database;
using Careblock.Model.Web.Medicine;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IMedicineTypeService
{
    Task<List<MedicineType>> GetAll();
    Task<MedicineType> GetById(Guid id); 
    Task<int> Create(MedicineTypeFormDto medicineType);
    Task<MedicineTypeDto> Update(int id, MedicineTypeFormDto medicineType);
    Task<bool> Delete(int id);
}