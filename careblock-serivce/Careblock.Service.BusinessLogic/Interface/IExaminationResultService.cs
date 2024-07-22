using Careblock.Model.Database;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IExaminationResultService
{
    Task<byte[]?> GetFileByPatientID(Guid patientId);
    Task<Guid> Create(ExaminationResultFormDto appointment);
    Task<bool> Update(ExaminationResultDto appointment);
    Task<bool> Delete(Guid id);
}