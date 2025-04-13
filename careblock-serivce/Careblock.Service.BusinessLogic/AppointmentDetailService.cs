using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Web.Appointment;
using Careblock.Service.BusinessLogic.Common;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Payment;

namespace Careblock.Service.BusinessLogic;

public class AppointmentDetailService : EntityService<AppointmentDetail>, IAppointmentDetailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;
    private readonly IStorageService _storageService;
    private readonly IResultService _resultService; 
    private readonly IPaymentService _paymentService;

    public AppointmentDetailService(IUnitOfWork unitOfWork, DatabaseContext dbContext, IStorageService storageService, IResultService resultService, IPaymentService paymentService) : base(unitOfWork, unitOfWork.AppointmentDetailRepository)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _storageService = storageService;
        _resultService = resultService; 
        _paymentService = paymentService;
    }

    public async Task<List<AppointmentDetailDto>> GetAll()
    {
        return await _unitOfWork.AppointmentDetailRepository.GetAll().Select(x => new AppointmentDetailDto()
        {
            Id = x.Id,
            AppointmentId = x.AppointmentId,
            Diagnostic = x.Diagnostic,
            DoctorId = x.DoctorId,
            ExaminationOptionId = x.ExaminationOptionId,
            Price = x.Price
        }).ToListAsync();
    }

    public async Task<AppointmentDetailDto> GetById(Guid id)
    {
        var appointmentDetails = await _unitOfWork.AppointmentDetailRepository.GetByIdAsync(id);
        if (appointmentDetails == null) return new AppointmentDetailDto();

        return new AppointmentDetailDto()
        {
            Id = appointmentDetails.Id,
            AppointmentId = appointmentDetails.AppointmentId,
            Diagnostic = appointmentDetails.Diagnostic,
            DoctorId = appointmentDetails.DoctorId,
            ExaminationOptionId = appointmentDetails.ExaminationOptionId,
            Price = appointmentDetails.Price
        };
    }

    public async Task<AppointmentDetailDto> GetByAppointmentId(Guid appointmentId)
    {
        var appointmentDetails = await _unitOfWork.AppointmentDetailRepository.FirstOrDefaultAsync(x => Guid.Equals(x.AppointmentId, appointmentId));
        if (appointmentDetails == null) return new AppointmentDetailDto();

        return new AppointmentDetailDto()
        {
            Id = appointmentDetails.Id,
            AppointmentId = appointmentDetails.AppointmentId,
            Diagnostic = appointmentDetails.Diagnostic,
            DoctorId = appointmentDetails.DoctorId,
            ExaminationOptionId = appointmentDetails.ExaminationOptionId,
            Price = appointmentDetails.Price
        };
    }

    public async Task<Guid> Create(AppointmentDetailFormDto appointmentDetailForm)
    {
        try
        {
            // Move appointment to checked in tab
            var appointment = await _dbContext.Appointments.Where(x => Guid.Equals(x.Id, appointmentDetailForm.AppointmentId)).FirstAsync() ?? throw new AppException("Appointment not found");

            var theDoctor = await _dbContext.Accounts.Where(x => Guid.Equals(x.Id, appointmentDetailForm.DoctorId)).FirstAsync();
            
            appointment.Status = AppointmentStatus.CheckedIn;
            appointment.DoctorId = appointmentDetailForm.DoctorId;
            appointment.Doctor = theDoctor;

            _dbContext.Appointments.Update(appointment);

            // Get total price
            var totalPrice = _dbContext.ExaminationOptions.Join(_dbContext.ExaminationPackageOptions,
                        eo => eo.Id,
                        epo => epo.ExaminationOptionId,
                        (eo, epo) => new { eo, epo })
                        .Where(join => Guid.Equals(join.epo.ExaminationPackageId, appointment.ExaminationPackageId))
                        .Sum(join => join.eo.Price);

            // Insert appointment details into db
            var newAppointmentDetail = new AppointmentDetail()
            {
                Id = Guid.NewGuid(),
                ExaminationOptionId = appointmentDetailForm.ExaminationOptionId,
                AppointmentId = appointmentDetailForm.AppointmentId,
                DoctorId = appointmentDetailForm.DoctorId,
                Diagnostic = appointmentDetailForm.Diagnostic,
                Price = totalPrice,
            };
            await _dbContext.AppointmentDetails.AddAsync(newAppointmentDetail);

            // Insert file pdf to azure storage
            var pdfUrl = await _storageService.UploadFile(appointmentDetailForm.FilePDF);

            // Insert result into db
            await _resultService.Create(new ResultFormDto
            {
                Id = !string.IsNullOrEmpty(appointmentDetailForm.ResultId) ? Guid.Parse(appointmentDetailForm.ResultId) : Guid.NewGuid(),
                SignHash = appointmentDetailForm.SignHash,
                ManagerWalletAddress = appointmentDetailForm.ManagerWalletAddress,
                AppointmentId = appointmentDetailForm.AppointmentId,
                DiagnosticUrl = pdfUrl,
                HashName = appointmentDetailForm.ResultName,
                Message = string.Empty,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            });

            var examinationPackage = await _dbContext.ExaminationPackages.Where(x => Guid.Equals(x.Id, appointment.ExaminationPackageId)).FirstAsync() ?? throw new AppException("Examination Package not found");
            var paymentMethod = await _dbContext.PaymentMethods.FirstAsync() ?? throw new AppException("Payment Method not found");

            // Insert payment into db
            await _paymentService.Create(new PaymentFormDto
            {
                AppointmentId = appointmentDetailForm.AppointmentId,
                Name = examinationPackage.Name,
                PaymentMethodId = paymentMethod.Id,
                Status = PaymentValue.UnPaid,
                Total = totalPrice ?? 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            });

            await _dbContext.SaveChangesAsync();
            return newAppointmentDetail.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Guid.Empty;
        }
    }

    public async Task<bool> Update(AppointmentDetailDto appointmentDetails)
    {
        var app = await _unitOfWork.AppointmentDetailRepository.GetByIdAsync(appointmentDetails.Id) ?? throw new AppException("Appointment details not found");
        app.DoctorId = appointmentDetails.DoctorId;
        app.Price = appointmentDetails.Price;
        app.Diagnostic = appointmentDetails.Diagnostic;
        app.ExaminationOptionId = appointmentDetails.ExaminationOptionId;

        return await UpdateAsync(app);
    }

    public async Task<bool> Delete(Guid id)
    {
        var app = await _unitOfWork.AppointmentDetailRepository.GetByIdAsync(id);
        return app == null ? throw new AppException("Appointment details not found") : await DeleteById(id);
    }
}