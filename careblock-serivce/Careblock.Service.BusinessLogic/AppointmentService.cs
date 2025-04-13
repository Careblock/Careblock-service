using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;

namespace Careblock.Service.BusinessLogic;

public class AppointmentService : EntityService<Appointment>, IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public AppointmentService(IUnitOfWork unitOfWork, DatabaseContext dbContext) : base(unitOfWork, unitOfWork.AppointmentRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<List<AppointmentDto>> GetAll()
    {
        return await _unitOfWork.AppointmentRepository.GetAll().Where(x => x.IsDeleted == false).Select(x => new AppointmentDto()
        {
            Id = x.Id,
            DoctorId = x.DoctorId ?? Guid.Empty,
            PatientId = x.PatientId,
            Note = x.Note,
            Reason = x.Reason,
            Status = x.Status,
            Address = x.Address,
            Email = x.Email,
            EndDateExpectation = x.EndDateExpectation,
            EndDateReality = x.EndDateReality,
            Gender = x.Gender,
            Name = x.Name,
            Phone = x.Phone,
            StartDateExpectation = x.StartDateExpectation,
            StartDateReality = x.StartDateReality,
            Symptom = x.Symptom,
            CreatedDate = x.CreatedDate,
            ModifiedDate = x.ModifiedDate
        }).ToListAsync();
    }

    public async Task<AppointmentDto> GetById(Guid id)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted == true) return new AppointmentDto();

        return new AppointmentDto()
        {
            Id = appointment.Id,
            DoctorId = appointment.DoctorId ?? Guid.Empty,
            PatientId = appointment.PatientId,
            Note = appointment.Note,
            Reason = appointment.Reason,
            Status = appointment.Status,
            CreatedDate = appointment.CreatedDate,
            ModifiedDate = appointment.ModifiedDate,
            EndDateExpectation = appointment.EndDateExpectation,
            Address = appointment.Address,
            Email = appointment.Email,
            EndDateReality = appointment.EndDateReality,
            Gender = appointment.Gender,
            Name = appointment.Name,
            Phone = appointment.Phone,
            StartDateExpectation = appointment.StartDateExpectation,
            StartDateReality = appointment.StartDateReality,
            Symptom = appointment.Symptom,
            ExaminationPackageId = appointment.ExaminationPackageId,
            OrganizationId = appointment.OrganizationId,
        };
    }

    public async Task<List<AppointmentHistoryDto>> GetByPatientID(Guid patientId)
    {
        var appointments = await _dbContext.Appointments.Where(app => app.IsDeleted == false)
            .Include(a => a.Doctor)
            .Include(b => b.Organization)
            .Include(b => b.ExaminationPackage)
            .Include(d => d.Review)
            .Select(apm => new AppointmentHistoryDto
            {
                Id = apm.Id,
                DoctorId = apm.DoctorId ?? Guid.Empty,
                DoctorName = apm.Doctor != null ? ($"{apm.Doctor.Firstname} {apm.Doctor.Lastname}") : string.Empty,
                DoctorAvatar = apm.Doctor != null ? (apm.Doctor.Avatar) : string.Empty,
                OrganizationId = apm.OrganizationId,
                OrganizationName = apm.Organization.Name,
                ExaminationPackageId = apm.ExaminationPackage.Id,
                ExaminationPackageName = apm.ExaminationPackage.Name,
                PatientId = apm.PatientId,
                Name = apm.Name,
                Gender = apm.Gender,
                Phone = apm.Phone,
                Email = apm.Email,
                Address = apm.Address,
                Symptom = apm.Symptom,
                Status = apm.Status,
                Reason = apm.Reason,
                Note = apm.Note,
                CreatedDate = apm.CreatedDate,
                StartDateReality = apm.StartDateReality,
                EndDateReality = apm.EndDateReality,
                StartDateExpectation = apm.StartDateExpectation,
                EndDateExpectation = apm.EndDateExpectation, 
                Feedback = apm.Review != null ? apm.Review.Content : "", 
                Rating = apm.Review != null ? apm.Review.Rating : null, 
            }).Where(x => Guid.Equals(x.PatientId, patientId))
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();

        return appointments;
    }

    public async Task<int> GetNumberNotAssigned(Guid userId)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, userId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        var appointments = _dbContext.Appointments.AsNoTracking()
            .Where(app => app.IsDeleted == false && Guid.Equals(app.OrganizationId, organizationId) && app.DoctorId == null);

        return appointments.Count();
    }

    public async Task<AppointmentHistories> GetByOrganizationID(AppointmentRequest appointmentRequest)
    {
        var organizationId = await _dbContext.Accounts
            .Where(acc => Guid.Equals(acc.Id, appointmentRequest.UserId))
            .Join(_dbContext.Departments,
             acc => acc.DepartmentId,
             dep => dep.Id,
            (acc, dep) => dep.OrganizationId)
            .FirstOrDefaultAsync();

        var results = _dbContext.Results.AsNoTracking();
        var appointments = _dbContext.Appointments.AsNoTracking()
            .Include(app => app.Doctor)
            .Include(app => app.Organization)
            .Include(app => app.ExaminationPackage)
            .Where(app => app.IsDeleted == false && Guid.Equals(app.OrganizationId, organizationId) &&
                (app.Name.ToLower().Contains(appointmentRequest.Keyword) ||
                app.Phone.ToLower().Contains(appointmentRequest.Keyword) ||
                app.Email.ToLower().Contains(appointmentRequest.Keyword)) &&
                ((appointmentRequest.ExaminationTypeId == -1) ||
                (appointmentRequest.ExaminationTypeId != -1 && app.ExaminationPackage.ExaminationTypeId == appointmentRequest.ExaminationTypeId)) &&
                (appointmentRequest.CreatedDate == null || app.StartDateExpectation.Value.Date == appointmentRequest.CreatedDate.Value.Date) &&
                (Guid.Equals(appointmentRequest.DoctorId, Guid.Empty) || Guid.Equals(app.DoctorId, appointmentRequest.DoctorId)))
            .Select(apm => new AppointmentHistoryDto
            {
                Id = apm.Id,
                PatientWalletAddress = apm.Patient.WalletAddress,
                DoctorId = apm.DoctorId ?? Guid.Empty,
                DoctorName = apm.Doctor != null ? ($"{apm.Doctor.Firstname} {apm.Doctor.Lastname}") : string.Empty,
                DoctorAvatar = apm.Doctor != null ? (apm.Doctor.Avatar) : string.Empty,
                OrganizationId = apm.OrganizationId,
                OrganizationName = apm.Organization.Name,
                ExaminationPackageId = apm.ExaminationPackage.Id,
                ExaminationPackageName = apm.ExaminationPackage.Name,
                ExaminationTypeId = apm.ExaminationPackage.ExaminationTypeId,
                PatientId = apm.PatientId,
                Name = apm.Name,
                Gender = apm.Gender,
                Phone = apm.Phone,
                Email = apm.Email,
                Address = apm.Address,
                Symptom = apm.Symptom,
                Status = apm.Status,
                Reason = apm.Reason,
                Note = apm.Note,
                CreatedDate = apm.CreatedDate,
                StartDateReality = apm.StartDateReality,
                EndDateReality = apm.EndDateReality,
                StartDateExpectation = apm.StartDateExpectation,
                EndDateExpectation = apm.EndDateExpectation,
                Results = results.Where(x => x.AppointmentId == apm.Id).Select(x => new ResultDto
                {
                    AppointmentId = x.AppointmentId,
                    Id = x.Id,
                    SignHash = x.SignHash,
                    Status = x.Status,
                    HashName = x.HashName,
                }).ToList()
            })
            .OrderByDescending(x => x.CreatedDate);


        return new AppointmentHistories
        {
            Total = appointments.Count(),
            PageData = appointments.Skip((appointmentRequest.PageIndex - 1) * appointmentRequest.PageNumber)
            .Take(appointmentRequest.PageNumber).ToList(),
        };
    }

    public async Task<Guid> Create(AppointmentFormDto appointment)
    {
        try
        {
            var newAppointment = new Appointment()
            {
                Id = Guid.NewGuid(),
                PatientId = appointment.PatientId,
                OrganizationId = appointment.OrganizationId,
                ExaminationPackageId = appointment.ExaminationPackageId,
                Reason = appointment.Reason,
                Status = appointment.Status,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Address = appointment.Address,
                Email = appointment.Email,
                Gender = appointment.Gender,
                Name = appointment.Name,
                Phone = appointment.Phone,
                EndDateExpectation = appointment.EndDateExpectation,
                StartDateExpectation = appointment.StartDateExpectation,
            };

            await CreateAsync(newAppointment);
            return newAppointment.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Guid.Empty;
        }
    }

    public async Task<bool> UpdateStatus(AppointmentStatus status, Guid id)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(id) ?? throw new AppException("Appointment not found");
        app.Status = status;

        return await UpdateAsync(app);
    }

    public async Task<int> AssignDoctor(Guid appointmentId, NotAssignedRequest request)
    {
        var doctor = await _dbContext.Accounts.FindAsync(request.DoctorId) ?? throw new AppException("Doctor not found");

        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId) ?? throw new AppException("Appointment not found");
        app.DoctorId = request.DoctorId;
        app.Doctor = doctor;
        app.ModifiedDate = DateTime.Now;

        await UpdateAsync(app);
        
        return await GetNumberNotAssigned(request.DoctorId);
    }

    public async Task<bool> Update(AppointmentDto appointment)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointment.Id) ?? throw new AppException("Appointment not found");
        app.DoctorId = appointment.DoctorId;
        app.PatientId = appointment.PatientId;
        app.Note = appointment.Note;
        app.Reason = appointment.Reason;
        app.Status = appointment.Status;
        app.ModifiedDate = DateTime.Now;

        return await UpdateAsync(app);
    }

    public async Task<bool> Delete(Guid id)
    {
        var app = await _unitOfWork.AppointmentRepository.GetByIdAsync(id) ?? throw new AppException("Appointment not found");

        app.IsDeleted = true;

        await UpdateAsync(app);

        return true;
    }
}