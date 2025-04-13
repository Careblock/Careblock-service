using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Web.Department;
using Careblock.Model.Web.Examination;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class ExaminationPackageReviewService : EntityService<ExaminationPackageReview>, IExaminationPackageReviewService

{
    private readonly IUnitOfWork _unitOfWork;

    public ExaminationPackageReviewService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.ExaminationPackageReviewRepository)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ExaminationPackageReviewDto>> GetByExaminationPackageId(Guid id)
    {
        return await _unitOfWork.ExaminationPackageReviewRepository.GetAll().Where(x => x.Id == id).Select(x => new ExaminationPackageReviewDto()
        {
            Id = x.Id,
            Content = x.Content,
            ExaminationPackageId = x.ExaminationPackageId,
            ResultId = x.ResultId,
            UserId = x.UserId,
        }).ToListAsync();
    }

    public async Task<ExaminationPackageReviewDto> GetByAppointmentId(Guid appointmentId)
    {
        var result = await _unitOfWork.ExaminationPackageReviewRepository
        .GetAll()
        .Where(x => x.AppointmentId == appointmentId)
        .Select(x => new ExaminationPackageReviewDto
        {
            Id = x.Id,
            Content = x.Content,
            Rating = x.Rating, 
            ExaminationPackageId = x.ExaminationPackageId,
            ResultId = x.ResultId,
            UserId = x.UserId,
            AppointmentId = x.AppointmentId,
        })
        .FirstOrDefaultAsync();

        return result ?? new ExaminationPackageReviewDto();
    }

    public async Task<bool> Create(ExaminationPackageReviewCreationDto input)
    {
        var newExaminationPackageReview = new ExaminationPackageReview()
        {
            Id = Guid.NewGuid(),
            Content = input.Content,
            CreatedDate = DateTime.Now,
            ExaminationPackageId = input.ExaminationPackageId,
            ResultId = input.ResultId,
            AppointmentId = input.AppointmentId,
            UserId = input.UserId,
            Rating = input.Rating, 
            SignHash = input.SignHash,
        };

        await CreateAsync(newExaminationPackageReview);
        return true;
    }

    public async Task<bool> Update(Guid id, ExaminationPackageReviewCreationDto dto)
    {
        var review = await _unitOfWork.ExaminationPackageReviewRepository.GetByIdAsync(id) ?? throw new AppException("Feedback not found"); ;
        review.Content = dto.Content;
        review.ExaminationPackageId = dto.ExaminationPackageId;
        review.ResultId = dto.ResultId;
        review.AppointmentId = dto.AppointmentId;
        review.UserId = dto.UserId;
        review.Rating = dto.Rating;
        review.ModifiedDate = DateTime.UtcNow;

        return await UpdateAsync(review);
    }


    public async Task<bool> Delete(Guid id)
    {
        _ = await _unitOfWork.ExaminationPackageReviewRepository.GetByIdAsync(id)
                 ?? throw new AppException("Review of examination package not found");
        return await DeleteById(id);
    }
}