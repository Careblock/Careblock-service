using Careblock.Model.Web.Account;
using Careblock.Model.Web.MedicalService;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IMedicalServiceService
{
    Task<List<MedicalServiceDto>> GetAll();
    Task<MedicalServiceDto> GetById(Guid id);
    Task<Guid> Create(MedicalServiceFormDto medicalService);
    Task<bool> Update(MedicalServiceDto medicalService);
    Task<bool> Delete(Guid id);
    Task<List<MedicalServiceDto>> FilterByOrganization(Guid organizationID);
}