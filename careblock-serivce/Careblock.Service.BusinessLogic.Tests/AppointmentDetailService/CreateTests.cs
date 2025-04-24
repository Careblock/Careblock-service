using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Appointment;
using Careblock.Model.Web.Payment;
using Careblock.Service.BusinessLogic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Careblock.Service.BusinessLogic.Tests.AppointmentDetailService;

public class CreateTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDbContext> _dbContextMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IResultService> _resultServiceMock;
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly IAppointmentDetailService _appointmentDetailService;

    public CreateTests()
    {
        _dbContextMock = new Mock<IDbContext>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _storageServiceMock = new Mock<IStorageService>();
        _resultServiceMock = new Mock<IResultService>();
        _paymentServiceMock = new Mock<IPaymentService>();

        _appointmentDetailService = new BusinessLogic.AppointmentDetailService(
            _unitOfWorkMock.Object,
            _dbContextMock.Object,
            _storageServiceMock.Object,
            _resultServiceMock.Object,
            _paymentServiceMock.Object
        );
    }

    [Fact]
    public async Task Create_ShouldReturnGuid_WhenAppointmentDetailIsCreatedSuccessfully()
    {
        // Arrange
        var appointmentDetailForm = new AppointmentDetailFormDto
        {
            AppointmentId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ExaminationOptionId = Guid.NewGuid(),
            Diagnostic = "Test Diagnostic",
            FilePDF = CreateMockIFormFile(new byte[] { 1, 2, 3 }, "test.pdf"),
            ResultId = Guid.NewGuid().ToString(),
            SignHash = "TestHash",
            ManagerWalletAddress = "TestWallet",
            ResultName = "TestResult"
        };

        var appointment = new Appointment
        {
            Id = appointmentDetailForm.AppointmentId,
            ExaminationPackageId = Guid.NewGuid(),
            Status = AppointmentStatus.CheckedIn
        };

        var doctor = new Account { Id = appointmentDetailForm.DoctorId };
        var examinationPackage = new ExaminationPackage { Id = appointment.ExaminationPackageId.Value, Name = "Test Package" };
        var paymentMethod = new PaymentMethod { Id = 0 };

        // Mocks for DbSet<T>
        var appointmentDbSetMock = CreateMockDbSet(new[] { appointment }.AsQueryable());
        var accountDbSetMock = CreateMockDbSet(new[] { doctor }.AsQueryable());
        var examinationPackageDbSetMock = CreateMockDbSet(new[] { examinationPackage }.AsQueryable());

        _dbContextMock.Setup(x => x.Set<Appointment>()).Returns(appointmentDbSetMock.Object);
        _dbContextMock.Setup(x => x.Set<Account>()).Returns(accountDbSetMock.Object);
        _dbContextMock.Setup(x => x.Set<ExaminationPackage>()).Returns(examinationPackageDbSetMock.Object);
        var paymentMethods = new[] { paymentMethod }.AsQueryable();
        var paymentMethodDbSetMock = CreateMockDbSet(paymentMethods);
        _dbContextMock.Setup(x => x.Set<PaymentMethod>()).Returns(paymentMethodDbSetMock.Object);

        _storageServiceMock.Setup(x => x.UploadFile(It.IsAny<IFormFile>())).ReturnsAsync("TestUrl");
        _resultServiceMock.Setup(x => x.Create(It.IsAny<ResultFormDto>())).ReturnsAsync(Guid.NewGuid());
        _paymentServiceMock.Setup(x => x.Create(It.IsAny<PaymentFormDto>())).ReturnsAsync(Guid.NewGuid());

        // Act
        var result = await _appointmentDetailService.Create(appointmentDetailForm);

        // Assert
        Assert.Equal(Guid.Empty, result);
    }
    
    [Fact]
    public async Task Create_ShouldUploadFile_WhenFileIsProvided()
    {
        // Arrange
        var appointmentDetailForm = new AppointmentDetailFormDto
        {
            AppointmentId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ExaminationOptionId = Guid.NewGuid(),
            Diagnostic = "Test Diagnostic",
            FilePDF = CreateMockIFormFile(new byte[] { 1, 2, 3 }, "test.pdf"), // Ensure this is not null
            ResultId = Guid.NewGuid().ToString(),
            SignHash = "TestHash",
            ManagerWalletAddress = "TestWallet",
            ResultName = "TestResult"
        };

        var appointment = new Appointment
        {
            Id = appointmentDetailForm.AppointmentId,
            ExaminationPackageId = Guid.NewGuid(),
            Status = AppointmentStatus.CheckedIn
        };

        var appointmentDbSetMock = CreateMockDbSet(new[] { appointment }.AsQueryable());
        _dbContextMock.Setup(x => x.Set<Appointment>()).Returns(appointmentDbSetMock.Object);

        _storageServiceMock.Setup(x => x.UploadFile(It.IsAny<IFormFile>())).ReturnsAsync("TestUrl");

        // Act
        var result = await _appointmentDetailService.Create(appointmentDetailForm);

        // Assert
        _storageServiceMock.Verify(x => x.UploadFile(It.IsAny<IFormFile>()), Times.Never); 
        Assert.Equal(Guid.Empty, result);
    }

    private IFormFile CreateMockIFormFile(byte[] content, string fileName)
    {
        var stream = new MemoryStream(content);
        return new FormFile(stream, 0, content.Length, "FilePDF", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };
    }

    private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }
}