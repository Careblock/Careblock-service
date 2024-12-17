using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Department;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class DepartmentService : EntityService<Department>, IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly DatabaseContext _dbContext;

    public DepartmentService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.DepartmentRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _dbContext = dbContext;
    }

    public async Task<List<DepartmentDto>> GetAll()
    {
        return await _unitOfWork.DepartmentRepository.GetAll().Select(x => new DepartmentDto()
        {
            Id = x.Id,
            Name = x.Name,
            OrganizationId = x.OrganizationId,
            OrganizationName = x.Organization.Name,
            Location = x.Location,
            ModifiedDate = x.ModifiedDate,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<DepartmentDto> GetById(Guid id)
    {
        var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        if (department == null) return new DepartmentDto();

        return new DepartmentDto()
        {
            Id = department.Id,
            Name = department.Name,
            OrganizationId = department.OrganizationId,
            Location = department.Location,
        };
    }

    public async Task<List<DepartmentDto>> GetByUserId(Guid userId)
    {
        var organizationId = await _dbContext.Accounts
           .Where(acc => Guid.Equals(acc.Id, userId))
           .Join(_dbContext.Departments,
            acc => acc.DepartmentId,
            dep => dep.Id,
           (acc, dep) => dep.OrganizationId)
           .FirstOrDefaultAsync();

        if (Guid.Equals(organizationId, Guid.Empty)) return new List<DepartmentDto>();

        var result = await _unitOfWork.DepartmentRepository.GetAll().Where(x => Guid.Equals(x.OrganizationId, organizationId)).Select(dep => new DepartmentDto
        {
            Id = dep.Id,
            Name = dep.Name,
            OrganizationId = dep.OrganizationId,
            Location = dep.Location,
            ModifiedDate = dep.ModifiedDate,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();

        return result;
    }

    public async Task<List<DepartmentDto>> GetByOrganization(Guid organizationId)
    {
        var departments = await _unitOfWork.DepartmentRepository.GetAll().Where(x => x.OrganizationId == organizationId).Select(x => new DepartmentDto()
        {
            Id = x.Id,
            OrganizationId = x.OrganizationId,
            Name = x.Name,
            Location = x.Location,
            ModifiedDate = x.ModifiedDate,
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();

        if (departments == null) return new List<DepartmentDto>();
        return departments;
    }

    public async Task<Guid> Create(DepartmentFormDto department)
    {
        var newDepartment = new Department()
        {
            Id = Guid.NewGuid(),
            Name = department.Name,
            OrganizationId = department.OrganizationId,
            Location = department.Location,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        await CreateAsync(newDepartment);
        return newDepartment.Id;
    }

    public async Task<bool> Update(Guid id, DepartmentDto department)
    {
        var departmentRes = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        if (departmentRes == null)
            throw new AppException("Department not found");

        departmentRes.Name = department.Name;
        departmentRes.Location = department.Location;
        departmentRes.OrganizationId = department.OrganizationId;
        departmentRes.ModifiedDate = DateTime.Now;

        return await UpdateAsync(departmentRes);
    }

    public async Task<bool> Delete(Guid id)
    {
        var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        if (department == null)
            throw new AppException("Department not found");

        return await DeleteById(id);
    }
}