using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Service.BusinessLogic.Interface;
using Moq;
using Xunit;

namespace Careblock.Service.BusinessLogic.Tests.ResultService;

public class SendTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IDbContext> _dbContextMock;
    private readonly Mock<IResultRepository> _resultRepoMock;
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly BusinessLogic.ResultService _service;

    public SendTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _storageServiceMock = new Mock<IStorageService>();
        _dbContextMock = new Mock<IDbContext>();

        _resultRepoMock = new Mock<IResultRepository>();
        _accountRepoMock = new Mock<IAccountRepository>();

        _unitOfWorkMock.Setup(u => u.ResultRepository).Returns(_resultRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.AccountRepository).Returns(_accountRepoMock.Object);

        _service = new BusinessLogic.ResultService(_unitOfWorkMock.Object, _storageServiceMock.Object,
            _dbContextMock.Object);
    }

    [Fact]
    public async Task Send_ShouldReturnTrue_WhenResultIsFoundAndUpdated()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        var result = new Result { Id = resultId, Status = (int)ResultStatus.Pending }; // Adjust to your Result object
        _resultRepoMock.Setup(repo => repo.GetByIdAsync(resultId)).ReturnsAsync(result);
        
        // Act
        var resultStatus = await _service.Send(resultId);

        // Assert
        Assert.False(resultStatus);
        Assert.Equal((int)ResultStatus.Sent, result.Status);
        _resultRepoMock.Verify(repo => repo.GetByIdAsync(resultId), Times.Once);
    }

    [Fact]
    public async Task Send_ShouldThrowAppException_WhenResultNotFound()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        _resultRepoMock.Setup(repo => repo.GetByIdAsync(resultId)).ReturnsAsync((Result)null);

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(() => _service.Send(resultId));
        _resultRepoMock.Verify(repo => repo.GetByIdAsync(resultId), Times.Once); // Verify GetByIdAsync was called
    }

    [Fact]
    public async Task Send_ShouldUpdateStatusToSent_WhenResultIsFound()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        var result = new Result { Id = resultId, Status = (int)ResultStatus.Pending };
        _resultRepoMock.Setup(repo => repo.GetByIdAsync(resultId)).ReturnsAsync(result);
        
        // Act
        await _service.Send(resultId);

        // Assert
        Assert.Equal((int)ResultStatus.Sent, result.Status); // Ensure status is updated to Sent
        _resultRepoMock.Verify(repo => repo.GetByIdAsync(resultId), Times.Once); // Ensure GetByIdAsync was called
    }
}
