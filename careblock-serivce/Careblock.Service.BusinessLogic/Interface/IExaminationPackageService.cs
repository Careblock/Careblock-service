using Careblock.Model.Database;
using Careblock.Model.Web.Examination;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IExaminationPackageService
{
    Task<List<ExaminationPackageDto>> GetAll();
    Task<ExaminationPackage> GetById(Guid id);
    Task<List<ExaminationPackageResponseDto>> GetByType(int examinationTypeId);
    Task<List<ExaminationPackageResponseDto>> GetByTypeAndOrganization(int examinationTypeId, Guid organizationId);
    Task<List<ExaminationPackageResponseDto>> GetByOrganization(Guid userId);
    Task<Guid> Create(ExaminationPackageFormDto organization, Guid userId);
    Task<Guid> CreateNew(ExaminationPackageFormDto organization);
    Task<ExaminationPackageDto> Update(Guid id, ExaminationPackageFormDto examinationPackageFormDto);
    Task<bool> Delete(Guid id);
}