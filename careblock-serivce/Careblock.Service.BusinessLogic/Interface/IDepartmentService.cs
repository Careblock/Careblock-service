using Careblock.Model.Web.Department;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAll();
    Task<DepartmentDto> GetById(Guid id); 
    Task<List<DepartmentDto>> GetByUserId(Guid userId);
    Task<List<DepartmentDto>> GetByOrganization(Guid organizationId);
    Task<Guid> Create(DepartmentFormDto department);
    Task<bool> Update(Guid id, DepartmentDto department);
    Task<bool> Delete(Guid id);
}