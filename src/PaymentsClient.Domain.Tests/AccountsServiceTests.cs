using AutoFixture;
using Moq;
using NUnit.Framework;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AccountsServiceTests
{        
    private Fixture _fixture;
    private readonly Mock<INagApiClientService> _nagApiClientServiceMock;
    private AccountsService _accountsService;

    /// <summary>
    /// The attribute: [FixtureLifeCycle(LifeCycle.InstancePerTestCase)] triggers the constructor call for each test run
    /// </summary>
    public AccountsServiceTests()
    {
        _fixture = new Fixture();
        _nagApiClientServiceMock = new Mock<INagApiClientService>();
        _accountsService = new AccountsService(_nagApiClientServiceMock.Object);
    }
    
    [Test]
    public async Task GetAccountsAsync_WithValidRequest_ReturnsExpectedResult()
    {
        // Arrange
        var getAccountsRequest = _fixture.Create<GetAccountsRequest>();
        var accountFixture = _fixture.Create<AccountModel>();
        var getAccountResponse = new GetAccountsResponse() { Accounts = [accountFixture] };
        _nagApiClientServiceMock
            .Setup(x => x.GetAccountsAsync(getAccountsRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetAccountsResponse>.Success(getAccountResponse));

        // Act
        var result = await _accountsService.GetAccountsAsync(getAccountsRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo(getAccountResponse));
        _nagApiClientServiceMock
            .Verify(x => x.GetAccountsAsync(getAccountsRequest, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GetTransactionsAsync_WithValidRequest_ReturnsExpectedResult()
    {
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        var transactionFixture = _fixture.Create<TransactionModel>();
        var getTransactionsResponse = new GetTransactionsResponse() { Transactions = [transactionFixture] };
        _nagApiClientServiceMock
            .Setup(x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetTransactionsResponse>.Success(getTransactionsResponse));

        // Act
        var result = await _accountsService.GetTransactionsAsync(getTransactionsRequest);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.EqualTo(getTransactionsResponse));
        _nagApiClientServiceMock
            .Verify(x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GetAccountsAsync_WithCancellationToken_CancellationTokenIsPassed()
    {
        // Arrange
        var getAccountsRequest = _fixture.Create<GetAccountsRequest>();
        var accountFixture = _fixture.Create<AccountModel>();
        var getAccountResponse = new GetAccountsResponse() { Accounts = [accountFixture] };
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientServiceMock
            .Setup(x => x.GetAccountsAsync(getAccountsRequest, cancellationToken))
            .ReturnsAsync(Result<GetAccountsResponse>.Success(getAccountResponse));

        // Act
        var result = await _accountsService.GetAccountsAsync(getAccountsRequest, cancellationToken);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.EqualTo(getAccountResponse));
        _nagApiClientServiceMock
            .Verify(x => x.GetAccountsAsync(getAccountsRequest, cancellationToken)
                , Times.Once);
    }

    [Test]
    public async Task GetTransactionsAsync_WithCancellationToken_CancellationTokenIsPassed()
    {
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        var transactionFixture = _fixture.Create<TransactionModel>();
        var getTransactionsResponse = new GetTransactionsResponse() { Transactions = [transactionFixture] };
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientServiceMock
            .Setup(x => x.GetTransactionsAsync(getTransactionsRequest, cancellationToken))
            .ReturnsAsync(Result<GetTransactionsResponse>.Success(getTransactionsResponse));

        // Act
        var result = await _accountsService.GetTransactionsAsync(getTransactionsRequest, cancellationToken);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.EqualTo(getTransactionsResponse));
        _nagApiClientServiceMock
            .Verify(x => x.GetTransactionsAsync(getTransactionsRequest, cancellationToken)
                , Times.Once);
    }
    
    [Test]
    public async Task GetAccountsAsync_NagApiClientServiceFails_ReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Create<GetAccountsRequest>();
        var expectedResponse = Result<GetAccountsResponse>.Failure("There is an issue with your request, please verify the logs.");
        _nagApiClientServiceMock
            .Setup(x => x.GetAccountsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetAccountsAsync(request);

        // Assert
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientServiceMock
            .Verify(x => x.GetAccountsAsync(It.IsAny<GetAccountsRequest>(), It.IsAny<CancellationToken>())
                , Times.Once);
    }
    
    [Test]
    public async Task GetTransactionsAsync_NagApiClientServiceFails_ReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Create<GetTransactionsRequest>();
        var expectedResponse = Result<GetTransactionsResponse>.Failure("There is an issue with your request, please verify the logs.");
        _nagApiClientServiceMock
            .Setup(x => x.GetTransactionsAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountsService.GetTransactionsAsync(request);

        // Assert
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientServiceMock
            .Verify(x => x.GetTransactionsAsync(It.IsAny<GetTransactionsRequest>(), It.IsAny<CancellationToken>())
                , Times.Once);
    }
}