using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class PasswordHelperTests
{
    [Fact]
    public void Generate_DefaultLength_Returns12CharPassword()
    {
        var pwd = PasswordHelper.Generate();
        pwd.Should().HaveLength(12);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    public void Generate_CustomLength_ReturnsCorrectLength(int length)
    {
        PasswordHelper.Generate(length).Should().HaveLength(length);
    }

    [Fact]
    public void Generate_WithSymbols_ContainsSymbol()
    {
        // Run several times to reduce flakiness
        var results = Enumerable.Range(0, 20).Select(_ => PasswordHelper.Generate(12, useSymbols: true));
        results.Any(p => p.Any(c => "!@#$%^&*()-_=+[]{}|;:,.<>?".Contains(c))).Should().BeTrue();
    }

    [Fact]
    public void Generate_WithoutSymbols_ContainsNoSymbol()
    {
        for (int i = 0; i < 10; i++)
        {
            var pwd = PasswordHelper.Generate(16, useSymbols: false);
            pwd.Any(c => "!@#$%^&*()-_=+[]{}|;:,.<>?".Contains(c)).Should().BeFalse();
        }
    }

    [Fact]
    public void Generate_LengthBelowMinimum_Throws()
    {
        var act = () => PasswordHelper.Generate(3);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IsStrong_StrongPassword_ReturnsTrue()
    {
        PasswordHelper.IsStrong("Abc123!@#").Should().BeTrue();
    }

    [Theory]
    [InlineData("short1A!")]        // exactly 8 chars — strong
    [InlineData("abc")]             // too short
    [InlineData("alllowercase1!")]  // no uppercase
    [InlineData("ALLUPPERCASE1!")]  // no lowercase
    [InlineData("NoDigitHere!")]    // no digit
    [InlineData("NoSymbol123Ab")]   // no symbol
    public void IsStrong_WeakPasswords_ReturnsFalse(string password)
    {
        // "short1A!" has uppercase, lowercase, digit and symbol with length 8 — should be strong
        if (password == "short1A!")
            PasswordHelper.IsStrong(password).Should().BeTrue();
        else
            PasswordHelper.IsStrong(password).Should().BeFalse();
    }

    [Fact]
    public void IsStrong_NullOrEmpty_ReturnsFalse()
    {
        PasswordHelper.IsStrong("").Should().BeFalse();
        PasswordHelper.IsStrong("   ").Should().BeFalse();
    }

    [Fact]
    public void GetStrengthScore_StrongPassword_Returns4()
    {
        PasswordHelper.GetStrengthScore("MyStr0ng!Pass").Should().Be(4);
    }

    [Fact]
    public void GetStrengthScore_Empty_Returns0()
    {
        PasswordHelper.GetStrengthScore("").Should().Be(0);
    }

    [Fact]
    public void HashSha256_SameInput_ProducesSameHash()
    {
        var hash1 = PasswordHelper.HashSha256("my-password");
        var hash2 = PasswordHelper.HashSha256("my-password");
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void HashSha256_DifferentInputs_ProduceDifferentHashes()
    {
        PasswordHelper.HashSha256("password1").Should().NotBe(PasswordHelper.HashSha256("password2"));
    }

    [Fact]
    public void HashSha256_NullOrEmpty_Throws()
    {
        var act = () => PasswordHelper.HashSha256("");
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void VerifySha256_CorrectPassword_ReturnsTrue()
    {
        var hash = PasswordHelper.HashSha256("secret");
        PasswordHelper.VerifySha256("secret", hash).Should().BeTrue();
    }

    [Fact]
    public void VerifySha256_WrongPassword_ReturnsFalse()
    {
        var hash = PasswordHelper.HashSha256("secret");
        PasswordHelper.VerifySha256("wrong", hash).Should().BeFalse();
    }
}
