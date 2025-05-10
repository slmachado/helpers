namespace Helpers.Tests;

using FluentAssertions;
using Helpers;
using System;
using System.Net;
using System.Net.Security;
using Xunit;

public class SslHelperTests
{
    [Fact]
    public void IgnoreCertificateWarning_ShouldSetValidationCallbackToAlwaysReturnTrue()
    {
        // Arrange
        ServicePointManager.ServerCertificateValidationCallback = null;
        var fakeSender = new object(); // Mock sender
        var fakeCert = new System.Security.Cryptography.X509Certificates.X509Certificate(""u8.ToArray()); // Mock certificate
        var fakeChain = new System.Security.Cryptography.X509Certificates.X509Chain(); // Mock chain

        // Act
        SslHelper.IgnoreCertificateWarning();

        // Assert
        ServicePointManager.ServerCertificateValidationCallback.Should().NotBeNull();

        if (ServicePointManager.ServerCertificateValidationCallback is not null)
        {
            var callbackResult = ServicePointManager.ServerCertificateValidationCallback.Invoke(fakeSender, fakeCert, fakeChain, SslPolicyErrors.None);
            callbackResult.Should().BeTrue();
        }
    }


    [Fact]
    public void RestoreCertificateValidation_ShouldRestoreDefaultValidationCallback()
    {
        // Arrange
        var originalCallback = ServicePointManager.ServerCertificateValidationCallback;

        // Act
        SslHelper.IgnoreCertificateWarningTemporarily(() =>
        {
            // Dentro da ação: a callback deve ser a definida para ignorar certificados
            ServicePointManager.ServerCertificateValidationCallback.Should().NotBeNull();
            if (ServicePointManager.ServerCertificateValidationCallback != null)
            {
                var callbackResult = ServicePointManager.ServerCertificateValidationCallback.Invoke(
                    new object(),
                    new System.Security.Cryptography.X509Certificates.X509Certificate(new byte[] { }),
                    new System.Security.Cryptography.X509Certificates.X509Chain(),
                    SslPolicyErrors.None);
                callbackResult.Should().BeTrue();
            }
        });

        // Assert: Verificar se a callback foi restaurada ao estado original
        if (originalCallback is null)
        {
            ServicePointManager.ServerCertificateValidationCallback.Should().BeNull();
        }
        else
        {
            ServicePointManager.ServerCertificateValidationCallback.Should().Be(originalCallback);
        }
    }


    [Fact]
    public void SetCustomCertificateValidation_ShouldSetCustomValidationCallback()
    {
        // Arrange
        RemoteCertificateValidationCallback customCallback = (sender, cert, chain, sslPolicyErrors) => sslPolicyErrors == SslPolicyErrors.None;

        // Act
        SslHelper.SetCustomCertificateValidation(customCallback);

        // Assert
        ServicePointManager.ServerCertificateValidationCallback.Should().Be(customCallback);
    }

    [Fact]
    public void IgnoreCertificateWarningTemporarily_ShouldIgnoreWarningsOnlyDuringActionExecution()
    {
        // Arrange
        var originalCallback = ServicePointManager.ServerCertificateValidationCallback;
        var fakeSender = new object(); // Mock sender
        var fakeCert = new System.Security.Cryptography.X509Certificates.X509Certificate(""u8.ToArray()); // Mock certificate
        var fakeChain = new System.Security.Cryptography.X509Certificates.X509Chain(); // Mock chain

        // Act
        SslHelper.IgnoreCertificateWarningTemporarily(() =>
        {
            // Assert inside the action: verify that the callback is set to ignore warnings
            ServicePointManager.ServerCertificateValidationCallback.Should().NotBeNull();
            if (ServicePointManager.ServerCertificateValidationCallback != null)
            {
                var callbackResult = ServicePointManager.ServerCertificateValidationCallback.Invoke(
                    fakeSender, fakeCert, fakeChain, SslPolicyErrors.None);
                callbackResult.Should().BeTrue();
            }
        });

        // Assert after the action: verify that the original callback has been restored
        ServicePointManager.ServerCertificateValidationCallback.Should().Be(originalCallback);
    }
}
