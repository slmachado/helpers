using System;
using Helpers;
using FluentAssertions;
using Xunit;

namespace Helpers.Tests
{
    public class BooleanHelperTests
    {
        [Fact]
        public void GetBooleanIntValueFromString_ValidTrueString_ShouldReturnOne()
        {
            BooleanHelper.GetBooleanIntValueFromString("true").Should().Be(1);
            BooleanHelper.GetBooleanIntValueFromString("1").Should().Be(1);
        }

        [Fact]
        public void GetBooleanIntValueFromString_ValidFalseString_ShouldReturnZero()
        {
            BooleanHelper.GetBooleanIntValueFromString("false").Should().Be(0);
            BooleanHelper.GetBooleanIntValueFromString("0").Should().Be(0);
        }

        [Fact]
        public void GetBooleanIntValueFromString_InvalidString_ShouldThrowArgumentException()
        {
            Action act = () => BooleanHelper.GetBooleanIntValueFromString("invalid");
            act.Should().Throw<ArgumentException>().WithMessage("The input value is not a valid boolean representation");
        }

        [Fact]
        public void TryParseBooleanIntValueFromString_ValidTrueString_ShouldReturnTrueAndOne()
        {
            BooleanHelper.TryParseBooleanIntValueFromString("true", out var boolValue).Should().BeTrue();
            boolValue.Should().Be(1);
        }

        [Fact]
        public void TryParseBooleanIntValueFromString_ValidFalseString_ShouldReturnTrueAndZero()
        {
            BooleanHelper.TryParseBooleanIntValueFromString("false", out var boolValue).Should().BeTrue();
            boolValue.Should().Be(0);
        }

        [Fact]
        public void TryParseBooleanIntValueFromString_InvalidString_ShouldReturnFalse()
        {
            BooleanHelper.TryParseBooleanIntValueFromString("invalid", out _).Should().BeFalse();
        }

        [Fact]
        public void GetBooleanFromString_ValidTrueString_ShouldReturnTrue()
        {
            BooleanHelper.GetBooleanFromString("true").Should().BeTrue();
            BooleanHelper.GetBooleanFromString("1").Should().BeTrue();
        }

        [Fact]
        public void GetBooleanFromString_ValidFalseString_ShouldReturnFalse()
        {
            BooleanHelper.GetBooleanFromString("false").Should().BeFalse();
            BooleanHelper.GetBooleanFromString("0").Should().BeFalse();
        }

        [Fact]
        public void GetBooleanFromString_InvalidString_ShouldThrowArgumentException()
        {
            Action act = () => BooleanHelper.GetBooleanFromString("invalid");
            act.Should().Throw<ArgumentException>().WithMessage("The input value is not a valid boolean representation");
        }

        [Fact]
        public void TryParseBooleanFromString_ValidTrueString_ShouldReturnTrueAndTrue()
        {
            BooleanHelper.TryParseBooleanFromString("true", out var boolValue).Should().BeTrue();
            boolValue.Should().BeTrue();
        }

        [Fact]
        public void TryParseBooleanFromString_ValidFalseString_ShouldReturnTrueAndFalse()
        {
            BooleanHelper.TryParseBooleanFromString("false", out var boolValue).Should().BeTrue();
            boolValue.Should().BeFalse();
        }

        [Fact]
        public void TryParseBooleanFromString_InvalidString_ShouldReturnFalse()
        {
            BooleanHelper.TryParseBooleanFromString("invalid", out _).Should().BeFalse();
        }

        [Fact]
        public void GetBooleanFromString_CustomTrueFalseString_ValidTrueString_ShouldReturnTrue()
        {
            BooleanHelper.GetBooleanFromString("yes", "yes", "no").Should().BeTrue();
        }

        [Fact]
        public void GetBooleanFromString_CustomTrueFalseString_ValidFalseString_ShouldReturnFalse()
        {
            BooleanHelper.GetBooleanFromString("no", "yes", "no").Should().BeFalse();
        }

        [Fact]
        public void GetBooleanFromString_CustomTrueFalseString_InvalidString_ShouldThrowArgumentException()
        {
            Action act = () => BooleanHelper.GetBooleanFromString("maybe", "yes", "no");
            act.Should().Throw<ArgumentException>().WithMessage("The input value is not a valid boolean representation");
        }

        [Fact]
        public void TryParseBooleanFromString_CustomTrueFalseString_ValidTrueString_ShouldReturnTrueAndTrue()
        {
            BooleanHelper.TryParseBooleanFromString("yes", "yes", "no", out var boolValue).Should().BeTrue();
            boolValue.Should().BeTrue();
        }

        [Fact]
        public void TryParseBooleanFromString_CustomTrueFalseString_ValidFalseString_ShouldReturnTrueAndFalse()
        {
            BooleanHelper.TryParseBooleanFromString("no", "yes", "no", out var boolValue).Should().BeTrue();
            boolValue.Should().BeFalse();
        }

        [Fact]
        public void TryParseBooleanFromString_CustomTrueFalseString_InvalidString_ShouldReturnFalse()
        {
            BooleanHelper.TryParseBooleanFromString("maybe", "yes", "no", out _).Should().BeFalse();
        }

        [Fact]
        public void FormatBooleanToString_TrueValue_ShouldReturnCustomTrueString()
        {
            BooleanHelper.FormatBooleanToString(true, "yes", "no").Should().Be("yes");
        }

        [Fact]
        public void FormatBooleanToString_FalseValue_ShouldReturnCustomFalseString()
        {
            BooleanHelper.FormatBooleanToString(false, "yes", "no").Should().Be("no");
        }
    }
}
