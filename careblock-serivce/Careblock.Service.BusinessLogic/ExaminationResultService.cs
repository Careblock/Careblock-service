using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Linq.Expressions;

namespace Careblock.Service.BusinessLogic;

public class ExaminationResultService : EntityService<ExaminationResult>, IExaminationResultService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public ExaminationResultService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.ExaminationResultRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<byte[]?> GetFileByPatientID(Guid patientId)
    {
        //Expression<Func<ExaminationResult, bool>> predicate = p => (Guid.Equals(patientId, p.PatientId));
        //var result = _unitOfWork.ExaminationResultRepository.FindBy(predicate).Select(x => new ExaminationResultDto
        //{
        //    Id = x.PatientId,
        //    PatientId = x.PatientId,
        //    Key = x.Key,
        //    Value = x.Value,
        //    CreatedDate = x.CreatedDate,
        //    ModifiedDate = x.ModifiedDate,
        //}).FirstOrDefault();

        string fileName = "examination-result-c2d299de-2f73-4297-8ba1-cd132632839a.pdf";
        var resultFile = _storageService.GetFile(fileName).Result;
        if (resultFile != null)
            return resultFile;

        return null;
    }

    public async Task<Guid> Create(ExaminationResultFormDto result)
    {
        var newResult = new ExaminationResult()
        {
            Id = Guid.NewGuid(),
            PatientId = result.PatientId,
            Key = result.Key,
            Value = result.Value,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        await _storageService.UploadFile(result.ResultFile);
        //await CreateAsync(newResult);
        return newResult.Id;
    }

    public async Task<bool> Update(ExaminationResultDto result)
    {
        var rs = await _unitOfWork.ExaminationResultRepository.GetByIdAsync(result.Id);
        if (rs == null)
            throw new AppException("Examinzation result not found");

        rs.Value = result.Value;
        rs.ModifiedDate = DateTime.Now;

        return await UpdateAsync(rs);
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _unitOfWork.ExaminationResultRepository.GetByIdAsync(id);
        if (result == null)
            throw new AppException("Examination result not found");

        return await DeleteById(id);
    }
}