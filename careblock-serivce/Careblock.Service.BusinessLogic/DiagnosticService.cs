using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;
using Careblock.Model.Web.Diagnostic;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Careblock.Service.BusinessLogic;

public class DiagnosticService : EntityService<Diagnostic>, IDiagnosticService
{
    private readonly IUnitOfWork _unitOfWork;

    public DiagnosticService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.DiagnosticRepository)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DiagnosticDto>> GetAll()
    {
        return await _unitOfWork.DiagnosticRepository.GetAll().Select(x => new DiagnosticDto()
        {
            Id = x.Id,
            BodyTemperature = x.BodyTemperature,
            DiastolicBloodPressure = x.DiastolicBloodPressure,
            Disease = x.Disease,
            DoctorId = x.DoctorId,
            HeartRate = x.HeartRate,
            Height = x.Height,
            Note = x.Note,
            PatientId = x.PatientId,
            Status = x.Status,
            SystolicBloodPressure = x.SystolicBloodPressure,
            Time = x.Time,
            Weight = x.Weight,
        }).ToListAsync();
    }

    public async Task<DiagnosticDto> GetById(Guid id)
    {
        var diagnostic = await _unitOfWork.DiagnosticRepository.GetByIdAsync(id);
        if (diagnostic == null) return new DiagnosticDto();

        return new DiagnosticDto()
        {
            Id = diagnostic.Id,
            BodyTemperature = diagnostic.BodyTemperature,
            DiastolicBloodPressure = diagnostic.DiastolicBloodPressure,
            Disease = diagnostic.Disease,
            DoctorId = diagnostic.DoctorId,
            HeartRate = diagnostic.HeartRate,
            Height = diagnostic.Height,
            Note = diagnostic.Note,
            PatientId = diagnostic.PatientId,
            Status = diagnostic.Status,
            SystolicBloodPressure = diagnostic.SystolicBloodPressure,
            Time = diagnostic.Time,
            Weight = diagnostic.Weight,
        };
    }


    public async Task<Guid> Create(DiagnosticFormDto diagnostic)
    {
        var newDiagnostic = new Diagnostic()
        {
            Id = Guid.NewGuid(),
            Weight = diagnostic.Weight,
            Time = diagnostic?.Time ?? DateTime.Now,
            SystolicBloodPressure = diagnostic.SystolicBloodPressure,
            Status = diagnostic.Status,
            PatientId = diagnostic.PatientId,
            Note = diagnostic.Note,
            Height = diagnostic.Height,
            BodyTemperature = diagnostic.BodyTemperature,
            DiastolicBloodPressure = diagnostic.DiastolicBloodPressure,
            Disease = diagnostic.Disease,
            DoctorId = diagnostic.DoctorId,
            HeartRate= diagnostic.HeartRate,
        };

        Expression<Func<Appointment, bool>> predicate = p => (p.PatientId == diagnostic.PatientId);
        var app = _unitOfWork.AppointmentRepository.FindBy(predicate).FirstOrDefault();

        if (app == null)
            throw new AppException("Appointment not found");
        else
        {
            app.Status = AppointmentStatus.CheckedIn;
            _unitOfWork.AppointmentRepository.UpdateAsync(app);
        }

        await CreateAsync(newDiagnostic);
        return newDiagnostic.Id;
    }

    public async Task<bool> Update(DiagnosticDto diagnostic)
    {
        var diag = await _unitOfWork.DiagnosticRepository.GetByIdAsync(diagnostic.Id);
        if (diag == null)
            throw new AppException("Diagnostic not found");

        diag.Time = diagnostic.Time;
        diag.Status = diagnostic.Status;
        diag.PatientId = diagnostic.PatientId;
        diag.DiastolicBloodPressure = diagnostic.DiastolicBloodPressure;
        diag.SystolicBloodPressure = diagnostic.SystolicBloodPressure;
        diag.BodyTemperature = diagnostic.BodyTemperature;
        diag.Disease = diagnostic.Disease;
        diag.DoctorId = diagnostic.DoctorId;
        diag.HeartRate = diagnostic.HeartRate;
        diag.Height = diagnostic.Height;
        diag.Id = diagnostic.Id;
        diag.Note = diagnostic.Note;
        diag.Weight = diagnostic.Weight;
        return await UpdateAsync(diag);
    }

    public async Task<bool> Delete(Guid id)
    {
        var diag = await _unitOfWork.DiagnosticRepository.GetByIdAsync(id);
        if (diag == null)
            throw new AppException("Diagnostic not found");

        return await DeleteById(id);
    }
}