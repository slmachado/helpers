using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Helpers;

namespace Helpers.Tests;

public class FluentHttpClientTests
{
    private class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; private set; }
        public string? ResponseContent { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            var response = new HttpResponseMessage(ResponseStatusCode)
            {
                Content = new StringContent(ResponseContent ?? "")
            };
            return await Task.FromResult(response);
        }
    }

    [Fact]
    public async Task GetAsync_CallsCorrectUrl_WithBaseUrlAndPath()
    {
        // Arrange
        var handler = new MockHttpMessageHandler { ResponseContent = "{}" };
        var httpClient = new HttpClient(handler);
        var client = new FluentHttpClient(httpClient)
            .WithBaseUrl("https://api.test.com");

        // Act
        await client.GetAsync<object>("users");

        // Assert
        handler.Request.Should().NotBeNull();
        handler.Request!.RequestUri!.ToString().Should().Be("https://api.test.com/users");
        handler.Request!.Method.Should().Be(HttpMethod.Get);
    }

    [Fact]
    public async Task WithHeader_AddsHeaderToRequest()
    {
        // Arrange
        var handler = new MockHttpMessageHandler { ResponseContent = "{}" };
        var httpClient = new HttpClient(handler);
        var client = new FluentHttpClient(httpClient)
            .WithHeader("X-Test-Header", "TestValue");

        // Act
        await client.GetAsync<object>("https://api.test.com/users");

        // Assert
        handler.Request!.Headers.GetValues("X-Test-Header").Should().Contain("TestValue");
    }

    [Fact]
    public async Task WithQueryParam_AddsQueryParametersToUrl()
    {
        // Arrange
        var handler = new MockHttpMessageHandler { ResponseContent = "{}" };
        var httpClient = new HttpClient(handler);
        var client = new FluentHttpClient(httpClient)
            .WithBaseUrl("https://api.test.com")
            .WithQueryParam("id", "123")
            .WithQueryParam("name", "test");

        // Act
        await client.GetAsync<object>("users");

        // Assert
        handler.Request!.RequestUri!.ToString().Should().Contain("id=123");
        handler.Request!.RequestUri!.ToString().Should().Contain("name=test");
    }

    [Fact]
    public async Task ExecuteRequest_ThrowsOnNonSuccessStatusCode()
    {
        // Arrange
        var handler = new MockHttpMessageHandler 
        { 
            ResponseStatusCode = HttpStatusCode.InternalServerError,
            ResponseContent = "Error message"
        };
        var httpClient = new HttpClient(handler);
        var client = new FluentHttpClient(httpClient);

        // Act
        var act = () => client.GetAsync<object>("https://api.test.com/users");

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*InternalServerError*Error message*");
    }

    [Fact]
    public async Task ExecuteRequest_DeserializesJsonResponse()
    {
        // Arrange
        var handler = new MockHttpMessageHandler 
        { 
            ResponseContent = "{\"id\": 1, \"name\": \"User 1\"}"
        };
        var httpClient = new HttpClient(handler);
        var client = new FluentHttpClient(httpClient);

        // Act
        var result = await client.GetAsync<TestUser>("https://api.test.com/users/1");

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result!.Name.Should().Be("User 1");
    }

    private class TestUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
