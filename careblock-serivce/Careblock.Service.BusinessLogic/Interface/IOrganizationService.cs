using Careblock.Model.Web.Organization;

namespace Careblock.Service.BusinessLogic.Interface;

public interface IOrganizationService
{
    Task<List<OrganizationDto>> GetAll();
    Task<OrganizationDto> GetById(Guid id);
    Task<OrganizationDto> GetByUserId(Guid userId);
    Task<Guid> Create(OrganizationFormDto organization);
    Task<OrganizationDto> Update(Guid id, OrganizationFormDto organization);
    Task<bool> Delete(Guid id);
}