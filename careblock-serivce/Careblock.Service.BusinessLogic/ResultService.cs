using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Examination;
using Careblock.Model.Web.Result;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Careblock.Service.BusinessLogic;

public class ResultService : EntityService<Result>, IResultService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    private readonly DatabaseContext _dbContext;

    public ResultService(IUnitOfWork unitOfWork, IStorageService storageService, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.ResultRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _dbContext = dbContext;
    }

    public async Task<List<Result>> GetByAppointment(Guid appointmentId)
    {
        var result = await _unitOfWork.ResultRepository.GetAll().Where(x => Guid.Equals(x.AppointmentId, appointmentId)).OrderByDescending(x => x.ModifiedDate).ToListAsync();

        return result;
    }

    public async Task<BillDto> GetBill(Guid appointmentId)
    {
        var result = await _dbContext.Appointments
        .Where(app => app.Id == appointmentId)
        .Select(app => new BillDto
        {
            PatientName = app.Name ?? (!string.IsNullOrWhiteSpace(app.Patient.Firstname) ? $"{app.Patient.Firstname} {app.Patient.Lastname}" : $"{app.Patient.Firstname}"),
            Gender = Utils.GetGenderName(app.Gender ?? Gender.Male),
            Phone = app.Phone,
            Address = app.Address,
            DoctorName = app.Doctor != null ? (!string.IsNullOrWhiteSpace(app.Doctor.Firstname) ? $"{app.Doctor.Firstname} {app.Doctor.Lastname}" : $"{app.Doctor.Firstname}") : string.Empty,
            DepartmentName = app.Doctor != null ? app.Doctor.Department.Name : string.Empty,
            OrganizationName = app.Organization.Name,
            CreatedDate = DateTime.Now,
            ExaminationPackageName = app.ExaminationPackage.Name,
            ExaminationOptions = app.ExaminationPackage.ExaminationPackageOptions.Select(x => x).Join(_dbContext.ExaminationOptions,
             epo => epo.ExaminationOptionId,
             exo => exo.Id,
            (epo, exo) => exo).Select(x => new ExaminationOptionDto
            {
                Id = x.Id,
                SpecialistId = x.SpecialistId,
                Name = x.Name,
                Price = x.Price,
            }).ToList(),
            TotalPrice = app.ExaminationPackage.ExaminationPackageOptions.Select(x => x).Join(_dbContext.ExaminationOptions,
             epo => epo.ExaminationOptionId,
             exo => exo.Id,
            (epo, exo) => exo).Select(x => new ExaminationOptionDto
            {
                Price = x.Price,
            }).Sum(opt => opt.Price) ?? 0
        }).FirstOrDefaultAsync();

        return result;
    }

    public async Task<Guid> Create(ResultFormDto result)
    {
        var newResult = new Result
        {
            Id = result.Id == Guid.Empty ? Guid.NewGuid() : result.Id,
            AppointmentId = result.AppointmentId,
            Message = string.Empty,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            DiagnosticUrl = result.DiagnosticUrl,
            Status = (int) ResultStatus.Pending, 
            SignHash = result.SignHash,
            HashName = result.HashName, 
        };

        var signerAddress = "addr_test1qzhtswd5f2fca8e0tea5jlmxs0petdt2zlv0d2xy9m7utzmcjnuv5q4jmja7q2r9t5szrc72sqt2wsczmlpd95z5x2tq8ctu7q";
        var appointment = await _unitOfWork.AppointmentRepository.GetAll()
            .Include(a => a.Doctor) 
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Id == result.AppointmentId) ?? throw new AppException("Appointment not found");

        var signerAddresses = new[] {appointment.Doctor.WalletAddress, signerAddress};

        var signers = _unitOfWork.AccountRepository.GetAll()
            .Where(account => signerAddresses.Contains(account.WalletAddress))
            .Select(account => new ResultMulSignDto
            {
                IsSigned = account.WalletAddress == appointment.Doctor.WalletAddress,
                SignerAddress = account.WalletAddress,
                SignerName = account.Lastname + account.Firstname,
                SignedDate = null
            })
            .ToList();

        newResult.MulSignJson = JsonSerializer.Serialize(signers, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }); 

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

    public async Task<bool> Sign(SignResultInputDto input)
    {
        var result = await _unitOfWork.ResultRepository.GetAll()
        .Include(x => x.Appointment)
        .Where(x => x.Id == input.ResultId)
        .FirstOrDefaultAsync() ?? throw new AppException("Result not found");

        var signer = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.Id == result.Appointment.DoctorId);
        var patient = await _unitOfWork.AccountRepository.FirstOrDefaultAsync(x => x.Id == result.Appointment.PatientId);

        var lstSigner = JsonSerializer.Deserialize<List<ResultMulSignDto>>(result.MulSignJson,
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        if ((lstSigner == null || !lstSigner.Any(x => x.SignerAddress == input.SignerAddress))
            && input.SignerAddress != signer?.WalletAddress)
        {
            throw new AppException("Signer not exist");
        }

        foreach (var item in lstSigner)
        {
            if (item.SignerAddress == input.SignerAddress)
            {
                item.IsSigned = true;
                item.SignedDate = DateTime.UtcNow;
            }
        }

        result.MulSignJson = JsonSerializer.Serialize(lstSigner, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        if (lstSigner.All(x => x.IsSigned))
        {
            result.Status = (int) ResultStatus.Signed;
            result.SignHash = input.SignHash;
            result.SignedDate = DateTime.Now;
        }
        else
        {
            result.Status = (int) ResultStatus.Pending;
        }

        result.ModifiedDate = DateTime.UtcNow;
        result.SignHash = input.SignHash;

        _unitOfWork.ResultRepository.UpdateAsync(result);
        return await _unitOfWork.CommitAsync() > 0;
    }

    public async Task<bool> Send(Guid id)
    {
        var result = await _unitOfWork.ResultRepository.GetByIdAsync(id) ?? throw new AppException("Result not found");
        result.Status = (int)ResultStatus.Sent;
        return await UpdateAsync(result);
    }

}