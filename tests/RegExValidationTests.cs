using FluentAssertions;

namespace Helpers.Tests;

public class RegExValidationTests
{
    [Theory]
    [InlineData("Password1@", true)]
    [InlineData("pass", false)]
    [InlineData("PASSWORD1", false)]
    [InlineData("Password@", false)]
    [InlineData("Password12345@", true)]
    public void CheckPassword_ShouldValidatePasswordCorrectly(string password, bool expected)
    {
        var result = RegExValidation.CheckPassword(password);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name+tag+sorting@example.com", true)]
    [InlineData("plainaddress", false)]
    [InlineData("@missingusername.com", false)]
    [InlineData("username@.com.my", false)]
    public void IsValidEmail_ShouldValidateEmailCorrectly(string email, bool expected)
    {
        var result = RegExValidation.IsValidEmail(email);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("+12345678901", true)]
    [InlineData("+112345678901234", true)]
    [InlineData("12345", false)]
    [InlineData("+1234567890123456", false)]
    [InlineData("abcd", false)]
    public void IsValidPhoneNumber_ShouldValidatePhoneNumberCorrectly(string phoneNumber, bool expected)
    {
        var result = RegExValidation.IsValidPhoneNumber(phoneNumber);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("http://example.com", true)]
    [InlineData("https://example.com", true)]
    [InlineData("ftp://example.com", false)]
    [InlineData("invalidurl", false)]
    [InlineData("https:/invalid.com", false)]
    public void IsValidURL_ShouldValidateUrlCorrectly(string url, bool expected)
    {
        var result = RegExValidation.IsValidURL(url);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("12345", true)]
    [InlineData("1234-5678", true)]
    [InlineData("123", false)]
    [InlineData("123456", false)]
    [InlineData("abcd-1234", false)]
    public void IsValidPostalCode_ShouldValidatePostalCodeCorrectly(string postalCode, bool expected)
    {
        var result = RegExValidation.IsValidPostalCode(postalCode);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("1234567812345670", true)]
    [InlineData("1234567812345678", false)]
    [InlineData("abcd123456781234", false)]
    [InlineData("1111222233334444", true)]
    public void IsValidCreditCard_ShouldValidateCreditCardCorrectly(string creditCardNumber, bool expected)
    {
        var result = RegExValidation.IsValidCreditCard(creditCardNumber);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("192.168.1.1", true)]
    [InlineData("255.255.255.255", true)]
    [InlineData("256.256.256.256", false)]
    [InlineData("192.168.1", false)]
    [InlineData("abcd", false)]
    public void IsValidIPv4_ShouldValidateIPv4Correctly(string ipAddress, bool expected)
    {
        var result = RegExValidation.IsValidIPv4(ipAddress);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true)]
    [InlineData("::1", true)]
    [InlineData("2001:db8::ff00:42:8329", true)]
    [InlineData("abcd", false)]
    [InlineData("2001:0db8:85a3::8a2e:370:7334:12345", false)]
    public void IsValidIPv6_ShouldValidateIPv6Correctly(string ipAddress, bool expected)
    {
        var result = RegExValidation.IsValidIPv6(ipAddress);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123-45-6789", true)]
    [InlineData("123456789", false)]
    [InlineData("12-345-6789", false)]
    [InlineData("abc-de-ghij", false)]
    public void IsValidSSN_ShouldValidateSSNCorrectly(string ssn, bool expected)
    {
        var result = RegExValidation.IsValidSSN(ssn);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("user123", true)]
    [InlineData("UsErNaMe16", true)]
    [InlineData("ab", false)]
    [InlineData("username_123", false)]
    [InlineData("longusername123456", false)]
    public void IsValidUsername_ShouldValidateUsernameCorrectly(string username, bool expected)
    {
        var result = RegExValidation.IsValidUsername(username);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("10/04/2024", new[] { "yyyy-MM-dd", "dd/MM/yyyy" }, true)] // Ajustado o formato esperado
    public void IsValidDate_ShouldValidateDateCorrectly(string date, string[] formats, bool expected)
    {
        var result = RegExValidation.IsValidDate(date, formats);
        result.Should().Be(expected);
    }

}

