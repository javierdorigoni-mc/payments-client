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
        _accountsService = new Mock<IAccountsService>();
        _accountsController = new AccountsController(_accountsService.Object);
    }

    [Test]
    public async Task GivenAccountsServiceReturnsSuccessful_WhenGetAccountsAsync_ThenResponds200OK()
    {
        // Arrange
        var getAccountRequest = _fixture.Create<GetAccountsRequest>();
        var getAccountResponse = _fixture.Create<Result<GetAccountsResponse>>();

        _accountsService
            .Setup(x => x.GetAccountsAsync(It.IsAny<GetAccountsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getAccountResponse);

        // Act
        var result = (ObjectResult?)(await _accountsController.GetAccountsAsync(getAccountRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        var payload = (GetAccountsResponse)result.Value;
        Assert.That(payload, Is.EqualTo(getAccountResponse.Value));
        _accountsService
            .Verify(
                x => x.GetAccountsAsync(getAccountRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
}