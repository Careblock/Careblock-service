using Careblock.Model.Web.Organization;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IOrganizationService
{
    Task<List<OrganizationDto>> GetAll();
    Task<OrganizationDto> GetById(Guid id);
    Task<Guid> Create(OrganizationFormDto organization);
    Task<bool> Update(OrganizationDto organization);
    Task<bool> Delete(Guid id);
}