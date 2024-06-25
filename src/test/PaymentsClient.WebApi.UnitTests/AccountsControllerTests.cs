using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentsClient.Domain;
using PaymentsClient.Domain.Accounts;
using PaymentsClient.WebApi.Controllers;

namespace PaymentsClient.WebApi.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AccountsControllerTests
{
    private readonly Fixture _fixture;
    private readonly AccountsController _accountsController;
    private readonly Mock<IAccountsService> _accountsService;

    public AccountsControllerTests()
    {
        _fixture = new Fixture();
        _accountsService = new Mock<IAccountsService>();
        _accountsController = new AccountsController(_accountsService.Object);
    }

    [Test]
    public async Task GivenAccountsServiceReturnsSuccessful_WhenGetAccountsAsync_ThenResponds200OK()
    {
        // Arrange
        var getAccountRequest = _fixture.Create<GetAccountsRequest>();
        var accountFixture = _fixture.Create<AccountModel>();
        var getAccountResponse = new GetAccountsResponse() { Accounts = [accountFixture] };
        _accountsService
            .Setup(x => x.GetAccountsAsync(It.IsAny<GetAccountsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetAccountsResponse>.Success(getAccountResponse));

        // Act
        var result = (ObjectResult?)(await _accountsController.GetAccountsAsync(getAccountRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        var payload = (GetAccountsResponse)result.Value;
        Assert.That(payload, Is.EqualTo(getAccountResponse));
        _accountsService
            .Verify(
                x => x.GetAccountsAsync(getAccountRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task GivenAccountsServiceReturnsFailure_WhenGetAccountsAsync_ThenResponds400BadRequest()
    {        
        // Arrange
        var getAccountRequest = _fixture.Create<GetAccountsRequest>();
        _accountsService
            .Setup(x => x.GetAccountsAsync(It.IsAny<GetAccountsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetAccountsResponse>.Failure("invalid request"));
        
        // Act
        var result = (ObjectResult?)(await _accountsController.GetAccountsAsync(getAccountRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("invalid request"));
        _accountsService
            .Verify(
                x => x.GetAccountsAsync(getAccountRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task GivenAccountsServiceReturnsSuccessful_WhenGetTransactionsAsync_ThenResponds200OK()
    {
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        var transactionFixture = _fixture.Create<TransactionModel>();
        var getTransactionsResponse = new GetTransactionsResponse() { Transactions = [transactionFixture] };
        _accountsService
            .Setup(x => x.GetTransactionsAsync(It.IsAny<GetTransactionsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetTransactionsResponse>.Success(getTransactionsResponse));

        // Act
        var result = (ObjectResult?)(await _accountsController.GetTransactionsAsync(getTransactionsRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        var payload = (GetTransactionsResponse)result.Value;
        Assert.That(payload, Is.EqualTo(getTransactionsResponse));
        _accountsService
            .Verify(
                x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task GivenTransactionsServiceReturnsFailure_WhenGetTransactionsAsync_ThenResponds400BadRequest()
    {        
        // Arrange
        var getTransactionsRequest = _fixture.Create<GetTransactionsRequest>();
        _accountsService
            .Setup(x => x.GetTransactionsAsync(It.IsAny<GetTransactionsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<GetTransactionsResponse>.Failure("invalid request"));
        
        // Act
        var result = (ObjectResult?)(await _accountsController.GetTransactionsAsync(getTransactionsRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("invalid request"));
        _accountsService
            .Verify(
                x => x.GetTransactionsAsync(getTransactionsRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
}