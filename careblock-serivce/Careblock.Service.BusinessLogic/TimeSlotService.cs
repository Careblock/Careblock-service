using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.TimeSlot;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class TimeSlotService : EntityService<TimeSlot>, ITimeSlotService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public TimeSlotService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.TimeSlotRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<TimeSlotResponseDto>> GetAll()
    {
        return await _unitOfWork.TimeSlotRepository.GetAll().Select(x => new TimeSlotResponseDto
        {
            Id = x.Id,
            CreatedDate = x.CreatedDate,
            EndTime = x.EndTime,
            ExaminationPackageId = x.ExaminationPackageId,
            ExaminationPackageName = x.ExaminationPackage.Name,
            ModifiedDate = x.ModifiedDate,
            Period = x.Period,
            StartTime = x.StartTime
        }).OrderByDescending(x => x.ModifiedDate).ToListAsync();
    }

    public async Task<TimeSlot> GetById(Guid id)
    {
        var timeSlot = await _unitOfWork.TimeSlotRepository.GetByIdAsync(id);
        if (timeSlot == null) return new TimeSlot();
        return timeSlot;
    }

    public async Task<Guid> Create(TimeSlotFormDto timeSlot)
    {
        try
        {
            var newTimeSlot = new TimeSlot()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                EndTime = timeSlot.EndTime,
                ModifiedDate = DateTime.Now,
                Period = timeSlot.Period,
                ExaminationPackageId = timeSlot.ExaminationPackageId,
                StartTime = timeSlot.StartTime,
            };

            await CreateAsync(newTimeSlot);

            return newTimeSlot.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Guid.Empty;
        }
    }

    public async Task<TimeSlotResponseDto> Update(Guid id, TimeSlotFormDto timeSlot)
    {
        var result = await _unitOfWork.TimeSlotRepository.GetByIdAsync(id) ?? throw new AppException("Time slot not found");

        result.ExaminationPackageId = timeSlot.ExaminationPackageId;
        result.StartTime = timeSlot.StartTime;
        result.EndTime = timeSlot.EndTime;
        result.Period = timeSlot.Period;
        result.ModifiedDate = DateTime.Now;

        await UpdateAsync(result);

        return new TimeSlotResponseDto
        {
            Id = result.Id,
            ModifiedDate = DateTime.Now,
            Period = result.Period,
            ExaminationPackageId = result.ExaminationPackageId,
            EndTime = result.EndTime,
            CreatedDate = DateTime.Now,
            StartTime = result.StartTime,
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var timeSlot = await _unitOfWork.TimeSlotRepository.GetByIdAsync(id) ?? throw new AppException("Time slot not found");

        return await DeleteById(id);
    }
}