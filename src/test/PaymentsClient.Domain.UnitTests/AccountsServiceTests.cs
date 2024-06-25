using AutoFixture;
using Moq;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AccountsServiceTests
{        
    private readonly Fixture _fixture;
    private readonly Mock<INagApiClientService> _nagApiClientService;
    private readonly AccountsService _accountsService;

    public AccountsServiceTests()
    {
        _fixture = new Fixture();
        _nagApiClientService = new Mock<INagApiClientService>();
        _accountsService = new AccountsService(_nagApiClientService.Object);
    }
    
    [Test]
    public async Task GivenValidRequest_WhenGetAccountsAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var getAccountsRequest = _fixture.Create<GetAccountsRequest>();
        var accountFixture = _fixture.Create<AccountModel>();
        var getAccountResponse = new GetAccountsResponse() { Accounts = [accountFixture] };
        _nagApiClientService
            .Setup(x => x.GetAccountsAsync(getAccountsRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetAccountsResponse>.Success(getAccountResponse));

        // Act
        var result = await _accountsService.GetAccountsAsync(getAccountsRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo(getAccountResponse));
        _nagApiClientService
            .Verify(x => x.GetAccountsAsync(getAccountsRequest, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenValidRequest_WhenGetTransactionsAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        var transactionFixture = _fixture.Create<TransactionModel>();
        var getTransactionsResponse = new GetTransactionsResponse() { Transactions = [transactionFixture] };
        _nagApiClientService
            .Setup(x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetTransactionsResponse>.Success(getTransactionsResponse));

        // Act
        var result = await _accountsService.GetTransactionsAsync(getTransactionsRequest);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(getTransactionsResponse, Is.EqualTo(result.Value));
        _nagApiClientService
            .Verify(x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenGetAccountsAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var getAccountsRequest = _fixture.Create<GetAccountsRequest>();
        var accountFixture = _fixture.Create<AccountModel>();
        var getAccountResponse = new GetAccountsResponse() { Accounts = [accountFixture] };
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.GetAccountsAsync(getAccountsRequest, cancellationToken))
            .ReturnsAsync(Result<GetAccountsResponse>.Success(getAccountResponse));

        // Act
        var result = await _accountsService.GetAccountsAsync(getAccountsRequest, cancellationToken);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(getAccountResponse, Is.EqualTo(result.Value));
        _nagApiClientService
            .Verify(x => x.GetAccountsAsync(getAccountsRequest, cancellationToken)
                , Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenGetTransactionsAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        var transactionFixture = _fixture.Create<TransactionModel>();
        var getTransactionsResponse = new GetTransactionsResponse() { Transactions = [transactionFixture] };
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.GetTransactionsAsync(getTransactionsRequest, cancellationToken))
            .ReturnsAsync(Result<GetTransactionsResponse>.Success(getTransactionsResponse));

        // Act
        var result = await _accountsService.GetTransactionsAsync(getTransactionsRequest, cancellationToken);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(getTransactionsResponse, Is.EqualTo(result.Value));
        _nagApiClientService
            .Verify(x => x.GetTransactionsAsync(getTransactionsRequest, cancellationToken)
                , Times.Once);
    }
}