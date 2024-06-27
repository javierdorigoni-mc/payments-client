using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class NagApiHttpClientServiceTestsBase
{
    protected readonly Mock<HttpMessageHandler> HttpMessageHandler;
    private readonly HttpClient _httpClient;
    protected Mock<ILogger<NagApiHttpClientService>> Logger;
    protected NagApiHttpClientService NagApiHttpClientService;
    protected Fixture FixtureBuilder;
    public NagApiHttpClientServiceTestsBase()
    {
        HttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(HttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.test.com")
        };
        _httpClient.DefaultRequestHeaders.Add("X-Client-Id", "some-client-id");
        _httpClient.DefaultRequestHeaders.Add("X-Client-Secret", "some-client-secret");
        
        Logger = new Mock<ILogger<NagApiHttpClientService>>();
        
        NagApiHttpClientService = new NagApiHttpClientService(_httpClient, Logger.Object);
        
        FixtureBuilder = new Fixture();
    }
}