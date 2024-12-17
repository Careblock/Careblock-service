using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class ResultService : EntityService<Result>, IResultService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public ResultService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.ResultRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<List<Result>> GetByAppointment(Guid appointmentId)
    {
        var result = await _unitOfWork.ResultRepository.GetAll().Where(x => Guid.Equals(x.AppointmentId, appointmentId)).OrderByDescending(x => x.ModifiedDate).ToListAsync();

        return result;
    }

    public async Task<Guid> Create(ResultFormDto result)
    {
        var newResult = new Result()
        {
            Id = Guid.NewGuid(),
            AppointmentId = result.AppointmentId,
            Message = string.Empty,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            DiagnosticUrl = result.DiagnosticUrl,
        };

        await CreateAsync(newResult);

        return newResult.Id;
    }

    public async Task<bool> Update(ResultDto result)
    {
        var rs = await _unitOfWork.ResultRepository.GetByIdAsync(result.Id) ?? throw new AppException("Result not found");
        rs.Message = result.Message;
        rs.DiagnosticUrl = result.DiagnosticUrl;
        rs.ModifiedDate = DateTime.Now;

        return await UpdateAsync(rs);
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _unitOfWork.ResultRepository.GetByIdAsync(id);
        return result == null ? throw new AppException("Result not found") : await DeleteById(id);
    }
}