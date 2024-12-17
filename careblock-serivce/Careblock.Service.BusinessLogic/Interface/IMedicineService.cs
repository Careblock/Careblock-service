using Careblock.Model.Database;
using Careblock.Model.Web.Medicine;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IMedicineService
{
    Task<List<MedicineDto>> GetAll();
    Task<Medicine> GetById(Guid id);
    Task<List<MedicineResponseDto>> GetByType(int medicineTypeId);
    Task<List<MedicineResponseDto>> GetByTypeAndOrganization(int medicineTypeId, Guid organizationId);
    Task<List<MedicineResponseDto>> GetByOrganization(Guid userId);
    Task<Guid> Create(MedicineFormDto medicine, Guid userId);
    Task<Guid> CreateNew(MedicineFormDto medicine);
    Task<MedicineDto> Update(Guid id, MedicineFormDto medicine);
    Task<bool> Delete(Guid id);
}