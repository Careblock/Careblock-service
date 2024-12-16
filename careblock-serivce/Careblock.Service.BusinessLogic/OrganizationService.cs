using Careblock.Data.Repository.Common.DbContext;
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
    private readonly DatabaseContext _dbContext;

    public OrganizationService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.OrganizationRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _dbContext = dbContext;
    }

    public async Task<List<OrganizationDto>> GetAll()
    {
        return await _unitOfWork.OrganizationRepository.GetAll().Select(x => new OrganizationDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Address = x.Address,
            City = x.City,
            District = x.District,
            MapUrl = x.MapUrl,
            Thumbnail = x.Thumbnail,
            Tel = x.Tel,
            Website = x.Website,
            Fax = x.Fax,
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
            Address = organization.Address,
            City = organization.City,
            District = organization.District,
            MapUrl = organization.MapUrl,
            Thumbnail = organization.Thumbnail,
            Tel = organization.Tel,
            Website = organization.Website,
            Fax = organization.Fax,
        };
    }

    public async Task<OrganizationDto> GetByUserId(Guid userId)
    {
        var organization = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => new { acc, dep })
            .Join(_dbContext.Organizations,
             combined => combined.dep.OrganizationId,
             org => org.Id,
            (combined, org) => org)
            .FirstOrDefaultAsync();

        if (organization == null) return new OrganizationDto();

        return new OrganizationDto()
        {
            Id = organization.Id,
            Code = organization.Code,
            Name = organization.Name,
            Address = organization.Address,
            City = organization.City,
            District = organization.District,
            MapUrl = organization.MapUrl,
            Thumbnail = organization.Thumbnail,
            Tel = organization.Tel,
            Website = organization.Website,
            Fax = organization.Fax,
        };
    }

    public async Task<Guid> Create(OrganizationFormDto organization)
    {
        var newOrganization = new Organization()
        {
            Id = Guid.NewGuid(),
            Code = organization.Code ?? string.Empty,
            Name = organization.Name,
            Address = organization.Address,
            City = organization.City,
            District = organization.District,
            MapUrl = organization.MapUrl,
            Tel = organization.Tel,
            Website = organization.Website,
            Thumbnail = await _storageService.UploadImage(organization.Thumbnail),
            Fax = organization.Fax
        };

        await CreateAsync(newOrganization);
        return newOrganization.Id;
    }

    public async Task<OrganizationDto> Update(Guid id, OrganizationFormDto organization)
    {
        var org = await _unitOfWork.OrganizationRepository.GetByIdAsync(id) ?? throw new AppException("Organization not found");
        org.Code = organization.Code ?? string.Empty;
        org.Name = organization.Name;
        org.Address = organization.Address;
        org.City = organization.City;
        org.District = organization.District;
        org.MapUrl = organization.MapUrl;
        org.Tel = organization.Tel;
        org.Website = organization.Website;
        org.Fax = organization.Fax;
        if (organization.Thumbnail != null) org.Thumbnail = await _storageService.UploadImage(organization.Thumbnail);

        await UpdateAsync(org);

        return new OrganizationDto
        {
            Id = id,
            Code = org.Code,
            Name = org.Name,
            Address = org.Address,
            City = org.City,
            District = org.District,
            MapUrl = org.MapUrl,
            Thumbnail = org.Thumbnail,
            Tel = org.Tel,
            Website = org.Website,
            Fax = org.Fax,
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var examinationTypeOrganization = await _dbContext.ExaminationTypeOrganizations.Where(ts => Guid.Equals(ts.OrganizationId, id)).ToListAsync();
        var medicineTypeOrganization = await _dbContext.MedicineTypeOrganizations.Where(eo => Guid.Equals(eo.OrganizationId, id)).ToListAsync();
        
        if (examinationTypeOrganization.Any())
        {
            _dbContext.ExaminationTypeOrganizations.RemoveRange(examinationTypeOrganization);
            await _dbContext.SaveChangesAsync();
        }
        if (medicineTypeOrganization.Any())
        {
            _dbContext.MedicineTypeOrganizations.RemoveRange(medicineTypeOrganization);
            await _dbContext.SaveChangesAsync();
        }

        var org = await _unitOfWork.OrganizationRepository.GetByIdAsync(id);
        if (org == null)
            throw new AppException("Organization not found");

        return await DeleteById(id);
    }
}