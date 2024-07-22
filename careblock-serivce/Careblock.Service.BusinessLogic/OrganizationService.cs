using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Organization;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class OrganizationService : EntityService<Organization>, IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public OrganizationService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.OrganizationRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }
    
    public async Task<List<OrganizationDto>> GetAll()
    {
        return await _unitOfWork.OrganizationRepository.GetAll().Select(x => new OrganizationDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Location = x.Location,
            Avatar = x.Avatar,
            Description = x.Description,
            CreatedDate = x.CreatedDate,
        }).ToListAsync();
    }
    
    public async Task<OrganizationDto> GetById(Guid id)
    {
        var organization = await _unitOfWork.OrganizationRepository.GetByIdAsync(id);
        if (organization == null) return new OrganizationDto();
        
        return new OrganizationDto()
        {
            Id = organization.Id,
            Code = organization.Code,
            Name = organization.Name,
            Location = organization.Location,
            Avatar = organization.Avatar,
            Description = organization.Description,
            CreatedDate = organization.CreatedDate,
        };
    }
    
    public async Task<Guid> Create(OrganizationFormDto organization)
    {
        var newOrganization = new Organization()
        {
            Id = Guid.NewGuid(),
            Code = organization.Code,
            Name = organization.Name,
            Location = organization.Location,
            Avatar = await _storageService.UploadImage(organization.Avatar),
            Description = organization.Description,
            CreatedDate = DateTime.Now,
        };
        
        await CreateAsync(newOrganization);
        return newOrganization.Id;
    }
    
    public async Task<bool> Update(OrganizationDto organization)
    {
        var org = await _unitOfWork.OrganizationRepository.GetByIdAsync(organization.Id);
        if (org == null)
            throw new AppException("Organization not found");
        
        org.Code = organization.Code;
        org.Name = organization.Name;
        org.Location = organization.Location;
        org.Avatar = organization.Avatar;
        org.Description = organization.Description;
        org.ModifiedDate = DateTime.UtcNow;
        
        return await UpdateAsync(org);
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var org = await _unitOfWork.OrganizationRepository.GetByIdAsync(id);
        if (org == null)
            throw new AppException("Organization not found");
        
        return await DeleteById(id);
    }
}