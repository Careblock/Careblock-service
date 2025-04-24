using System.Text.Json;
using Careblock.Data.Repository.Common.DbContext;
using Careblock.Data.Repository.Interface;
using Careblock.Model.Database;
using Careblock.Model.Shared.Common;
using Careblock.Model.Shared.Enum;
using Careblock.Model.Web.Result;
using Careblock.Service.BusinessLogic.Interface;
using Moq;
using Xunit;

namespace Careblock.Service.BusinessLogic.Tests.ResultService;

public class SignTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IDbContext> _dbContextMock;
    private readonly Mock<IResultRepository> _resultRepoMock;
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly BusinessLogic.ResultService _service;

    public SignTests()
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
    public async Task Sign_ThrowsException_WhenResultDoesNotExist()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        var input = new SignResultInputDto
        {
            ResultId = resultId,
            SignerAddress = "nonexistentSigner",
            SignHash = "hash123"
        };

        _resultRepoMock
            .Setup(r => r.GetAll())
            .Returns(new TestAsyncEnumerable<Result>(new List<Result>().AsQueryable()));

        // Act & Assert
        await Assert.ThrowsAsync<AppException>(() => _service.Sign(input));
    }

    [Fact]
    public async Task Sign_ThrowsException_WhenSignerAlreadySigned()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        var signerAddress = "addr1...";
        var signHash = "hash123";

        var mulSignList = new List<ResultMulSignDto>
        {
            new ResultMulSignDto { SignerAddress = signerAddress, IsSigned = true }
        };
        var json = JsonSerializer.Serialize(mulSignList,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var result = new Result
        {
            Id = resultId,
            MulSignJson = null,
            Status = (int)ResultStatus.Pending
        };

        var input = new SignResultInputDto
        {
            ResultId = resultId,
            SignerAddress = signerAddress,
            SignHash = signHash
        };

        _resultRepoMock
            .Setup(r => r.GetAll())
            .Returns(new TestAsyncEnumerable<Result>(new List<Result> { result }.AsQueryable()));

        _accountRepoMock
            .Setup(a => a.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Account, bool>>>(),
                default))
            .ReturnsAsync(new Account { WalletAddress = signerAddress });

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Sign(input));
    }

    [Fact]
    public async Task Sign_UpdatesSignHash_WhenSignerSignsSuccessfully()
    {
        // Arrange
        var resultId = Guid.NewGuid();
        var signerAddress = "addr1...";
        var signHash = "newHash";

        var mulSignList = new List<ResultMulSignDto>
        {
            new ResultMulSignDto { SignerAddress = signerAddress, IsSigned = false }
        };
        var json = JsonSerializer.Serialize(mulSignList,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var result = new Result
        {
            Id = resultId,
            MulSignJson = json,
            Status = (int)ResultStatus.Pending
        };

        var input = new SignResultInputDto
        {
            ResultId = resultId,
            SignerAddress = signerAddress,
            SignHash = signHash
        };

        _resultRepoMock
            .Setup(r => r.GetAll())
            .Returns(new TestAsyncEnumerable<Result>(new List<Result> { result }.AsQueryable()));

        _accountRepoMock
            .Setup(a => a.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Account, bool>>>(),
                default))
            .ReturnsAsync(new Account { WalletAddress = signerAddress });

        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        var success = await _service.Sign(input);

        // Assert
        Assert.True(success);
        Assert.Equal(signHash, result.SignHash);
    }
}