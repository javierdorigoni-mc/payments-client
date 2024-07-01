using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class NagApiHttpClientServiceTestsBase
{
    protected Mock<HttpMessageHandler> HttpMessageHandler;
    private HttpClient _httpClient;
    protected Mock<ILogger<NagApiHttpClientService>> Logger;
    protected NagApiHttpClientService NagApiHttpClientService;
    protected Fixture FixtureBuilder;
    
    [SetUp]
    public void Setup()
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