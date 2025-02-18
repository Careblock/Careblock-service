using Careblock.Model.Database;
using Careblock.Model.Web.Specialist;

namespace Careblock.Service.BusinessLogic.Interface;

public interface ISpecialistService
{
    Task<List<SpecialistDto>> GetAll();
    Task<Specialist> GetById(Guid id); 
    Task<List<Specialist>> GetByUserId(Guid userId); 
    Task<bool> AssignSpecialist(Guid userId, SpecialistRequest request); 
    Task<Guid> Create(SpecialistFormDto specialist);
    Task<SpecialistDto> Update(Guid id, SpecialistFormDto specialist);
    Task<bool> Delete(Guid id);
}