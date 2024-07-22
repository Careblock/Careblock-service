using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Account;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Web.MedicalService;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Careblock.Service.BusinessLogic;

public class AppointmentService : EntityService<Appointment>, IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public AppointmentService(IUnitOfWork unitOfWork, IStorageService storageService) : base(unitOfWork, unitOfWork.AppointmentRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<bool> UpdateStatus(AppointmentStatus status, Guid id)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        if (app == null)
            throw new AppException("Appointment not found");

        app.Status = status;

        return await UpdateAsync(app);
    }

    public async Task<List<AppointmentDto>> GetAll()
    {
        return await _unitOfWork.AppointmentRepository.GetAll().Select(x => new AppointmentDto()
        {
            Id = x.Id,
            DoctorId = x.DoctorId,
            PatientId = x.PatientId,
            EndTime = x.EndTime,
            StartTime = x.StartTime,
            Note = x.Note,
            Reason = x.Reason,
            Status = x.Status,
            CreatedDate = x.CreatedDate,
            ModifiedDate = x.ModifiedDate
        }).ToListAsync();
    }

    public async Task<AppointmentDto> GetById(Guid id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        if (appointment == null) return new AppointmentDto();

        return new AppointmentDto()
        {
            Id = appointment.Id,
            DoctorId = appointment.DoctorId,
            PatientId = appointment.PatientId,
            EndTime = appointment.EndTime,
            StartTime = appointment.StartTime,
            Note = appointment.Note,
            Reason = appointment.Reason,
            Status = appointment.Status,
            CreatedDate = appointment.CreatedDate,
            ModifiedDate = appointment.ModifiedDate
        };
    }

    public async Task<List<AppointmentHistoryDto>> GetByPatientID(Guid patientId)
    {
        Expression<Func<Appointment, bool>> predicate = p => (Guid.Equals(patientId, p.PatientId));
        var result = _unitOfWork.AppointmentRepository.FindBy(predicate).AsEnumerable().Join(
            _unitOfWork.AccountRepository.GetAll(),
            appointment => appointment.DoctorId,
            account => account.Id,
            (appointment, account) => new AppointmentHistoryDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                EndTime = appointment.EndTime,
                StartTime = appointment.StartTime,
                Note = appointment.Note,
                Reason = appointment.Reason,
                Status = appointment.Status,
                CreatedDate = appointment.CreatedDate,
                ModifiedDate = appointment.ModifiedDate,
                DoctorName = account.Firstname + " " + account.Lastname,
                OrganizationId = account.OrganizationId,
                HospitalName = "",
                DoctorAvatar = account.Avatar,
            }).Join(_unitOfWork.OrganizationRepository.GetAll(),
            appointment => appointment.OrganizationId,
            organization => organization.Id,
            (appointment, organization) => new AppointmentHistoryDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                EndTime = appointment.EndTime,
                StartTime = appointment.StartTime,
                Note = appointment.Note,
                Reason = appointment.Reason,
                Status = appointment.Status,
                CreatedDate = appointment.CreatedDate,
                ModifiedDate = appointment.ModifiedDate,
                DoctorName = appointment.DoctorName,
                OrganizationId = appointment.OrganizationId,
                HospitalName = organization.Name,
                DoctorAvatar = appointment.DoctorAvatar,
            }).OrderBy(x => x.CreatedDate).ToList();
        return result;
    }

    public async Task<Guid> Create(AppointmentFormDto appointment)
    {
        var newAppointment = new Appointment()
        {
            Id = Guid.NewGuid(),
            DoctorId = appointment.DoctorId,
            PatientId = appointment.PatientId,
            EndTime = appointment.EndTime,
            StartTime = appointment.StartTime,
            Note = appointment.Note,
            Reason = appointment.Reason,
            Status = appointment.Status,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        await CreateAsync(newAppointment);
        return newAppointment.Id;
    }

    public async Task<bool> Update(AppointmentDto appointment)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointment.Id);
        if (app == null)
            throw new AppException("Appointment not found");

        app.DoctorId = appointment.DoctorId;
        app.PatientId = appointment.PatientId;
        app.EndTime = appointment.EndTime;
        app.StartTime = appointment.StartTime;
        app.Note = appointment.Note;
        app.Reason = appointment.Reason;
        app.Status = appointment.Status;
        app.ModifiedDate = DateTime.Now;

        return await UpdateAsync(app);
    }

    public async Task<bool> Delete(Guid id)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        if (app == null)
            throw new AppException("Appointment not found");

        return await DeleteById(id);
    }
}