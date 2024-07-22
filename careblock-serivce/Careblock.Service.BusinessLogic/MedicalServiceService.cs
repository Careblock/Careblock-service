using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.MedicalService;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Careblock.Service.BusinessLogic;

public class MedicalServiceService : EntityService<MedicalService>, IMedicalServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public MedicalServiceService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.MedicalServiceRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }
    
    public async Task<List<MedicalServiceDto>> GetAll()
    {
        return await _unitOfWork.MedicalServiceRepository.GetAll().Select(x => new MedicalServiceDto()
        {
            Id = x.Id,
            Avatar = x.Avatar,
            Name = x.Name,
            Note = x.Note,
            OrganizationId = x.OrganizationId,
            Price = x.Price
        }).ToListAsync();
    }
    
    public async Task<MedicalServiceDto> GetById(Guid id)
    {
        var medicalService = await _unitOfWork.MedicalServiceRepository.GetByIdAsync(id);
        if (medicalService == null) return new MedicalServiceDto();
        
        return new MedicalServiceDto()
        {
            Id = medicalService.Id,
            Avatar = medicalService.Avatar,
            Name = medicalService.Name,
            Note = medicalService.Note,
            OrganizationId = medicalService.OrganizationId,
            Price = medicalService.Price
        };
    }


    public async Task<List<MedicalServiceDto>> FilterByOrganization(Guid organizationID)
    {
        Expression<Func<MedicalService, bool>> predicate = p => (p.IsDeleted == false && p.OrganizationId != Guid.Empty && Guid.Equals(organizationID, p.OrganizationId));
        var result = await _unitOfWork.MedicalServiceRepository.FindBy(predicate).Select(x => new MedicalServiceDto
        {
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            Name = x.Name,
            Note = x.Note,
            Price = x.Price,
            OrganizationId = x.OrganizationId,
            Avatar = x.Avatar,
        }).ToListAsync();
        return result;
    }

    public async Task<Guid> Create(MedicalServiceFormDto medicalService)
    {
        var newMedicalService = new MedicalService()
        {
            Id = Guid.NewGuid(),
            Avatar = await _storageService.UploadImage(medicalService.Avatar),
            IsDeleted = medicalService.IsDeleted,
            Name = medicalService.Name,
            Note = medicalService.Note,
            OrganizationId = medicalService.OrganizationId,
            Price = medicalService.Price
        };
        
        await CreateAsync(newMedicalService);
        return newMedicalService.Id;
    }
    
    public async Task<bool> Update(MedicalServiceDto medicalService)
    {
        var medicalS = await _unitOfWork.MedicalServiceRepository.GetByIdAsync(medicalService.Id);
        if (medicalS == null)
            throw new AppException("MedicalService not found");

        medicalS.Note = medicalService.Note;
        medicalS.Price = medicalService.Price;
        medicalS.IsDeleted = medicalService.IsDeleted;
        medicalS.Avatar = medicalService.Avatar;
        medicalS.Name = medicalService.Name;

        return await UpdateAsync(medicalS);
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var medicalS = await _unitOfWork.MedicalServiceRepository.GetByIdAsync(id);
        if (medicalS == null)
            throw new AppException("MedicalService not found");
        
        return await DeleteById(id);
    }
}