namespace Helpers.Tests;

using FluentAssertions;
using Helpers;
using System;
using System.Collections.Generic;
using Xunit;

public class UriHelperTests
{
    [Fact]
    public void GetUri_ShouldConstructNewUri_WithSpecifiedIPAndPort()
    {
        // Arrange
        var originalUri = new Uri("http://example.com:8080/some/path");
        string ip = "192.168.1.1";
        string port = "9090";

        // Act
        var newUri = UriHelper.GetUri(originalUri, ip, port);

        // Assert
        newUri.Should().Be(new Uri("http://192.168.1.1:9090/some/path"));
    }

    [Fact]
    public void AddOrUpdateQueryParam_ShouldAddQueryParam_WhenNotPresent()
    {
        // Arrange
        var originalUri = new Uri("http://example.com/some/path");
        string paramName = "foo";
        string paramValue = "bar";

        // Act
        var newUri = UriHelper.AddOrUpdateQueryParam(originalUri, paramName, paramValue);

        // Assert
        newUri.Query.Should().Be("?foo=bar");
    }

    [Fact]
    public void AddOrUpdateQueryParam_ShouldUpdateQueryParam_WhenAlreadyPresent()
    {
        // Arrange
        var originalUri = new Uri("http://example.com/some/path?foo=oldValue");
        string paramName = "foo";
        string paramValue = "newValue";

        // Act
        var newUri = UriHelper.AddOrUpdateQueryParam(originalUri, paramName, paramValue);

        // Assert
        newUri.Query.Should().Be("?foo=newValue");
    }

    [Fact]
    public void RemoveQueryParam_ShouldRemoveQueryParam_WhenPresent()
    {
        // Arrange
        var originalUri = new Uri("http://example.com/some/path?foo=bar&baz=qux");
        string paramName = "foo";

        // Act
        var newUri = UriHelper.RemoveQueryParam(originalUri, paramName);

        // Assert
        newUri.Query.Should().Be("?baz=qux");
    }

    [Fact]
    public void RemoveQueryParam_ShouldDoNothing_WhenParamNotPresent()
    {
        // Arrange
        var originalUri = new Uri("http://example.com/some/path?baz=qux");
        string paramName = "foo";

        // Act
        var newUri = UriHelper.RemoveQueryParam(originalUri, paramName);

        // Assert
        newUri.Query.Should().Be("?baz=qux");
    }

    [Fact]
    public void IsValidUri_ShouldReturnTrue_ForValidUri()
    {
        // Arrange
        string validUri = "http://example.com/some/path";

        // Act
        var result = UriHelper.IsValidUri(validUri);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidUri_ShouldReturnFalse_ForInvalidUri()
    {
        // Arrange
        string invalidUri = "invalid_uri";

        // Act
        var result = UriHelper.IsValidUri(invalidUri);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHostAndPort_ShouldReturnCorrectHostAndPort()
    {
        // Arrange
        var uri = new Uri("http://example.com:8080/some/path");

        // Act
        var (host, port) = UriHelper.GetHostAndPort(uri);

        // Assert
        host.Should().Be("example.com");
        port.Should().Be(8080);
    }

    [Fact]
    public void BuildUri_ShouldConstructUri_FromSeparateParts()
    {
        // Arrange
        string scheme = "https";
        string host = "example.com";
        int port = 443;
        string path = "/index.html";
        var queryParams = new Dictionary<string, string>
        {
            { "foo", "bar" },
            { "baz", "qux" }
        };

        // Act
        var newUri = UriHelper.BuildUri(scheme, host, port, path, queryParams);

        // Assert
        newUri.Should().Be(new Uri("https://example.com:443/index.html?foo=bar&baz=qux"));
    }

    [Fact]
    public void BuildUri_ShouldHandleEmptyQueryParams()
    {
        // Arrange
        string scheme = "http";
        string host = "example.com";
        int port = 80;
        string path = "/";

        // Act
        var newUri = UriHelper.BuildUri(scheme, host, port, path, new Dictionary<string, string>());

        // Assert
        newUri.Should().Be(new Uri("http://example.com:80/"));
    }
}

