using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class NagApiHttpClientServiceTestsBase
{
    protected readonly Mock<HttpMessageHandler> HttpMessageHandler;
    protected HttpClient HttpClient;
    protected Mock<ILogger<NagApiHttpClientService>> Logger;
    protected NagApiHttpClientService NagApiHttpClientService;

    public NagApiHttpClientServiceTestsBase()
    {
        HttpMessageHandler = new Mock<HttpMessageHandler>();
        HttpClient = new HttpClient(HttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.test.com")
        };
        HttpClient.DefaultRequestHeaders.Add("X-Client-Id", "some-client-id");
        HttpClient.DefaultRequestHeaders.Add("X-Client-Secret", "some-client-secret");
        
        Logger = new Mock<ILogger<NagApiHttpClientService>>();
        NagApiHttpClientService = new NagApiHttpClientService(HttpClient, Logger.Object);
    }
}