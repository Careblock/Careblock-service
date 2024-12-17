using Careblock.Model.Database;
using Careblock.Model.Web.Examination;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IExaminationOptionService
{
    Task<List<ExaminationOptionDto>> GetAll();
    Task<List<ExaminationOption>> GetByPackage(Guid appointmentId);
    Task<ExaminationOption> GetById(Guid id); 
    Task<Guid> Create(ExaminationOptionFormDto examinationOption);
    Task<ExaminationOptionDto> Update(Guid id, ExaminationOptionFormDto examinationOption);
    Task<bool> Delete(Guid id);
}