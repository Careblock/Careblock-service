using Careblock.Model.Web.Diagnostic;
using Careblock.Model.Web.MedicalService;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IDiagnosticService
{
    Task<List<DiagnosticDto>> GetAll();
    Task<DiagnosticDto> GetById(Guid id);
    Task<Guid> Create(DiagnosticFormDto medicalService);
    Task<bool> Update(DiagnosticDto medicalService);
    Task<bool> Delete(Guid id);
}