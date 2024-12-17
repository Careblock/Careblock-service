using Careblock.Model.Database;
using Careblock.Model.Web.Examination;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IExaminationTypeService
{
    Task<List<ExaminationType>> GetAll();
    Task<ExaminationType> GetById(Guid id); 
    Task<List<ExaminationType>> GetByUserId(Guid id);
    Task<int> Create(ExaminationTypeFormDto examinationType);
    Task<ExaminationTypeDto> Update(int id, ExaminationTypeFormDto examinationType);
    Task<bool> Delete(int id);
}