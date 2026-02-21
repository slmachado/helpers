using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class GuidHelperTests
{
    [Theory]
    [InlineData("3f2504e0-4f89-11d3-9a0c-0305e82c3301", true)]
    [InlineData("not-a-guid", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_ReturnsExpected(string? input, bool expected)
    {
        GuidHelper.IsValid(input).Should().Be(expected);
    }

    [Fact]
    public void ToShortGuid_And_FromShortGuid_RoundTrip()
    {
        var original = Guid.NewGuid();
        var shortGuid = GuidHelper.ToShortGuid(original);
        var restored = GuidHelper.FromShortGuid(shortGuid);

        shortGuid.Should().HaveLength(22);
        restored.Should().Be(original);
    }

    [Fact]
    public void ToShortGuid_DoesNotContainBase64SpecialChars()
    {
        for (int i = 0; i < 10; i++)
        {
            var shortGuid = GuidHelper.ToShortGuid(Guid.NewGuid());
            shortGuid.Should().NotContain("/").And.NotContain("+").And.NotContain("=");
        }
    }

    [Fact]
    public void FromShortGuid_InvalidInput_ThrowsFormatException()
    {
        var act = () => GuidHelper.FromShortGuid("invalid!!!");
        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void FromShortGuid_NullOrEmpty_ThrowsArgumentNullException()
    {
        var act = () => GuidHelper.FromShortGuid("");
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ParseOrEmpty_ValidGuid_ReturnsParsedGuid()
    {
        var guid = Guid.NewGuid();
        GuidHelper.ParseOrEmpty(guid.ToString()).Should().Be(guid);
    }

    [Fact]
    public void ParseOrEmpty_Invalid_ReturnsGuidEmpty()
    {
        GuidHelper.ParseOrEmpty("not-valid").Should().Be(Guid.Empty);
        GuidHelper.ParseOrEmpty(null).Should().Be(Guid.Empty);
    }

    [Fact]
    public void TryParse_ValidGuid_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        GuidHelper.TryParse(guid.ToString()).Should().Be(guid);
    }

    [Fact]
    public void TryParse_Invalid_ReturnsNull()
    {
        GuidHelper.TryParse("bad-input").Should().BeNull();
        GuidHelper.TryParse(null).Should().BeNull();
    }

    [Fact]
    public void GenerateSequential_ReturnsUniqueGuids()
    {
        var g1 = GuidHelper.GenerateSequential();
        var g2 = GuidHelper.GenerateSequential();

        g1.Should().NotBe(g2);
    }

    [Fact]
    public void GenerateSequential_IsValidGuid()
    {
        var g = GuidHelper.GenerateSequential();
        g.Should().NotBe(Guid.Empty);
    }
}
