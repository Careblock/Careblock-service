using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Examination;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IExaminationPackageReviewService
{
    Task<List<ExaminationPackageReviewDto>> GetByExaminationPackageId(Guid id);
    Task<ExaminationPackageReviewDto> GetByAppointmentId(Guid id);
    Task<bool> Create(ExaminationPackageReviewCreationDto input);
    Task<bool> Update(Guid id, ExaminationPackageReviewCreationDto dto);
    Task<bool> Delete(Guid id);
}