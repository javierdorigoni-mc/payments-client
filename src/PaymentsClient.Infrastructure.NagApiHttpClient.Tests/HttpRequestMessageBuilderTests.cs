using System.Text.Json;
using AutoFixture.NUnit3;
using NUnit.Framework;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

[TestFixture]
public class HttpRequestMessageBuilderTests
{
    [Test]
    public void Create_ShouldReturnNewInstance()
    {
        // Act
        var builder = HttpRequestMessageBuilder.Create();

        // Assert
        Assert.That(builder, Is.Not.Null);
    }

    [Test]
    [InlineAutoData("GET")]
    [InlineAutoData("PUT")]
    [InlineAutoData("POST")]
    [InlineAutoData("DELETE")]
    [InlineAutoData("HEAD")]
    [InlineAutoData("OPTIONS")]
    [InlineAutoData("TRACE")]
    [InlineAutoData("PATCH")]
    [InlineAutoData("CONNECT")]
    public void WithHttpMethod_ShouldSetHttpMethod(string httpMethodName)
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var httpMethod = HttpMethod.Parse(httpMethodName);
        
        // Act
        builder.WithHttpMethod(httpMethod);
        var request = builder.Build();

        // Assert
        Assert.That(request.Method, Is.EqualTo(httpMethod));
    }

    [Test]
    public void WithRequestUri_ShouldSetRequestUri()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var baseUrl = "https://example.com/resource";

        // Act
        builder.WithRequestUri(baseUrl);
        var request = builder.Build();

        // Assert
        Assert.That(request.RequestUri?.ToString(), Is.EqualTo(baseUrl));
    }

    [Test]
    public void WithOptionalQueryStringParameter_ShouldAddQueryParameter()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var baseUrl = "https://example.com/resource";
        builder.WithRequestUri(baseUrl);

        // Act
        builder.WithOptionalQueryStringParameter("param", "value");
        var request = builder.Build();

        // Assert
        Assert.That(request.RequestUri?.ToString(), Is.EqualTo($"{baseUrl}?param=value"));
    }

    [Test]
    [InlineAutoData(null)]
    [InlineAutoData("")]
    [InlineAutoData("   ")]
    public void WithOptionalQueryStringParameter_ShouldNotAddQueryParameterWhenValueIsNullOrWhitespace(string? value)
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var baseUrl = "https://example.com/resource";
        builder.WithRequestUri(baseUrl);

        // Act
        builder.WithOptionalQueryStringParameter("param", value);
        var request = builder.Build();

        // Assert
        Assert.That(request.RequestUri?.ToString(), Is.EqualTo(baseUrl));
    }

    [Test]
    public void WithJsonSerializedContent_ShouldSetContent()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var content = new { Name = "Test" };

        // Act
        builder.WithJsonSerializedContent(content);
        var request = builder.Build();

        // Assert
        var requestContent = request.Content?.ReadAsStringAsync().Result;
        var expectedContent = JsonSerializer.Serialize(content, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.That(requestContent, Is.EqualTo(expectedContent));
    }

    [Test]
    public void WithAuthorizationBearerTokenHeader_ShouldSetAuthorizationHeader()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var token = "test-token";

        // Act
        builder.WithAuthorizationBearerTokenHeader(token);
        var request = builder.Build();

        // Assert
        Assert.That(request.Headers.Authorization?.ToString(), Is.EqualTo($"Bearer {token}"));
    }

    [Test]
    public void Build_DefaultMethodShouldBePost()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();

        // Act
        var request = builder.Build();

        // Assert
        Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
    }

    [Test]
    public void Build_ShouldIncludeAllSetProperties()
    {
        // Arrange
        var builder = HttpRequestMessageBuilder.Create();
        var uri = "https://example.com/resource";
        var token = "test-token";
        var content = new { Name = "Test" };

        // Act
        builder
            .WithHttpMethod(HttpMethod.Put)
            .WithRequestUri(uri)
            .WithAuthorizationBearerTokenHeader(token)
            .WithJsonSerializedContent(content)
            .WithOptionalQueryStringParameter("param", "value");
        var request = builder.Build();

        // Assert
        Assert.That(Equals(HttpMethod.Put, request.Method), Is.True);
        Assert.That(request.RequestUri?.ToString(), Is.EqualTo($"{uri}?param=value"));
        Assert.That(request.Headers.Authorization?.ToString(), Is.EqualTo($"Bearer {token}"));

        var requestContent = request.Content?.ReadAsStringAsync().Result;
        var expectedContent = JsonSerializer.Serialize(content, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.That(requestContent, Is.EqualTo(expectedContent));
    }
}