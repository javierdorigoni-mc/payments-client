using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AccountsServiceTests
{        
    private Fixture _fixture;
    private readonly Mock<INagApiClientService> _nagApiClientService;
    private readonly AccountsService _accountsService;

    public AccountsServiceTests()
    {
        _nagApiClientService = new Mock<INagApiClientService>();
        _accountsService = new AccountsService(_nagApiClientService.Object);
    }
    
    [Test]
    public async Task GivenValidRequest_WhenGetAccountsAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Create<GetAccountsRequest>();
        var expectedResponse = _fixture.Create<Result<GetAccountsResponse>>();
        _nagApiClientService
            .Setup(x => x.GetAccountsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetAccountsAsync(request);

        // Assert
        Assert.That(expectedResponse.Equals(result), Is.True);
        _nagApiClientService
            .Verify(x => x.GetAccountsAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenValidRequest_WhenGetTransactionsAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Create<GetTransactionsRequest>();
        var expectedResponse = _fixture.Create<Result<GetTransactionsResponse>>();
        _nagApiClientService
            .Setup(x => x.GetTransactionsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetTransactionsAsync(request);

        // Assert
        Assert.That(expectedResponse, Is.EqualTo(result));
        _nagApiClientService
            .Verify(x => x.GetTransactionsAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenGetAccountsAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<GetAccountsRequest>();
        var expectedResponse = _fixture.Create<Result<GetAccountsResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.GetAccountsAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetAccountsAsync(request, cancellationToken);

        // Assert
        Assert.That(expectedResponse, Is.EqualTo(result));
        _nagApiClientService
            .Verify(x => x.GetAccountsAsync(request, cancellationToken)
                , Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenGetTransactionsAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<GetTransactionsRequest>();
        var expectedResponse = _fixture.Create<Result<GetTransactionsResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.GetTransactionsAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetTransactionsAsync(request, cancellationToken);

        // Assert
        Assert.That(expectedResponse, Is.EqualTo(result));
        _nagApiClientService.Verify(x => x.GetTransactionsAsync(request, cancellationToken), Times.Once);
    }
}