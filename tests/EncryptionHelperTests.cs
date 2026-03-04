using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class EncryptionHelperTests
{
    [Fact]
    public void Encrypt_And_Decrypt_ShouldReturnOriginalText()
    {
        // Arrange
        var originalText = "Sensitive Data 123!";
        var password = "StrongPassword#1";

        // Act
        var encrypted = EncryptionHelper.Encrypt(originalText, password);
        var decrypted = EncryptionHelper.Decrypt(encrypted, password);

        // Assert
        decrypted.Should().Be(originalText);
    }

    [Fact]
    public void Decrypt_WithWrongPassword_ShouldThrowException()
    {
        // Arrange
        var originalText = "Sensitive Data";
        var password = "CorrectPassword";
        var wrongPassword = "WrongPassword";

        var encrypted = EncryptionHelper.Encrypt(originalText, password);

        // Act & Assert
        // Since it's AES, a wrong password will most likely fail during decryption 
        // leading to either a padding error or garbage data.
        var act = () => EncryptionHelper.Decrypt(encrypted, wrongPassword);
        act.Should().Throw<Exception>(); 
    }

    [Fact]
    public void Encrypt_DifferentSalts_ShouldProduceDifferentCiphertext()
    {
        // Arrange
        var text = "Data";
        var password = "Pass";

        // Act
        var encrypted1 = EncryptionHelper.Encrypt(text, password);
        var encrypted2 = EncryptionHelper.Encrypt(text, password);

        // Assert
        encrypted1.Should().NotBe(encrypted2);
    }
}
