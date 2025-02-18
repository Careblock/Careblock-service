using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class ExaminationOptionService : EntityService<ExaminationOption>, IExaminationOptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public ExaminationOptionService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.ExaminationOptionRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<ExaminationOptionDto>> GetAll()
    {
        return await _unitOfWork.ExaminationOptionRepository.GetAll().Select(x => new ExaminationOptionDto
        {
            Description = x.Description,
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            SpecialistId = x.SpecialistId,
            TimeEstimation = x.TimeEstimation,
            SpecialistName = x.Specialist.Name,
        }).ToListAsync();
    }

    public async Task<List<ExaminationOption>> GetByPackage(Guid appointmentId)
    {
        try
        {
            var examinationPackageId = _dbContext.Appointments.Where(x => Guid.Equals(x.Id, appointmentId)).First().ExaminationPackageId ?? throw new AppException("Appointment not found");

            var result = await _dbContext.ExaminationOptions.Join(_dbContext.ExaminationPackageOptions, eo => eo.Id, epo => epo.ExaminationOptionId, (eo, epo) => new { eo, epo }).Where(x => Guid.Equals(x.epo.ExaminationPackageId, examinationPackageId)).Select(x => new ExaminationOption
            {
                Description = x.eo.Description,
                ExaminationForm = x.eo.ExaminationForm,
                Id = x.eo.Id,
                Name = x.eo.Name,
                Price = x.eo.Price,
                SpecialistId = x.eo.SpecialistId,
                TimeEstimation = x.eo.TimeEstimation,
            }).ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            return new List<ExaminationOption>();
        }
    }

    public async Task<ExaminationOption> GetById(Guid id)
    {
        var examinationOption = await _unitOfWork.ExaminationOptionRepository.GetByIdAsync(id);
        if (examinationOption == null) return new ExaminationOption();
        return examinationOption;
    }

    public async Task<Guid> Create(ExaminationOptionFormDto examinationOption)
    {
        try
        {
            var newExaminationOption = new ExaminationOption()
            {
                Id = Guid.NewGuid(),
                Name = examinationOption.Name,
                Description = examinationOption.Description,
                ExaminationForm = examinationOption.ExaminationForm ?? string.Empty,
                Price = examinationOption.Price,
                TimeEstimation = examinationOption.TimeEstimation,
                SpecialistId = examinationOption.SpecialistId,
            };

            await CreateAsync(newExaminationOption);

            return newExaminationOption.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Guid.Empty;
        }
    }

    public async Task<ExaminationOptionDto> Update(Guid id, ExaminationOptionFormDto examinationOption)
    {
        var examOption = await _unitOfWork.ExaminationOptionRepository.GetByIdAsync(id) ?? throw new AppException("Examination option not found");

        examOption.Name = examinationOption.Name;
        examOption.SpecialistId = examinationOption.SpecialistId;
        examOption.Price = examinationOption.Price;
        examOption.Description = examinationOption.Description;
        examOption.ExaminationForm = examinationOption.ExaminationForm ?? string.Empty;
        examOption.TimeEstimation = examinationOption.TimeEstimation;

        await UpdateAsync(examOption);

        return new ExaminationOptionDto
        {
            Name = examOption.Name,
            Price = examOption.Price,
            Description = examOption.Description,
            ExaminationForm = examOption.ExaminationForm,
            SpecialistId = examinationOption.SpecialistId,
            TimeEstimation = examOption.TimeEstimation
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var examinationOption = await _unitOfWork.ExaminationOptionRepository.GetByIdAsync(id) ?? throw new AppException("Examination option not found");

        return await DeleteById(id);
    }
}