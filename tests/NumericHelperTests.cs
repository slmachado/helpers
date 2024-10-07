using FluentAssertions;

namespace Helpers.Tests;

public class NumericHelperTests
{
    [Theory]
    [InlineData("123.456", 123.456)]
    [InlineData("0.123", 0.123)]
    [InlineData("-123.456", -123.456)]
    [InlineData("123,456", 123.456)] // Testing comma replacement
    public void GetDoubleFromString_ShouldConvertStringToDouble(string input, double expected)
    {
        var result = NumericHelper.GetDoubleFromString(input);
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void GetDoubleFromString_InvalidInput_ShouldThrowArgumentException(string input)
    {
        Action act = () => NumericHelper.GetDoubleFromString(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("123.456", 123.456, true)]
    [InlineData("abc", 0.0, false)]
    public void TryGetDoubleFromString_ShouldReturnExpectedResult(string input, double expected, bool success)
    {
        var result = NumericHelper.TryGetDoubleFromString(input, out double value);
        result.Should().Be(success);
        if (success)
        {
            value.Should().BeApproximately(expected, 0.0001);
        }
    }

    [Theory]
    [InlineData("123.45", 123.45f)]
    [InlineData("-123.45", -123.45f)]
    [InlineData("0.45", 0.45f)]
    public void GetFloatFromString_ShouldConvertStringToFloat(string input, float expected)
    {
        var result = NumericHelper.GetFloatFromString(input);
        result.Should().BeApproximately(expected, 0.0001f);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void GetFloatFromString_InvalidInput_ShouldThrowArgumentException(string input)
    {
        Action act = () => NumericHelper.GetFloatFromString(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    public void GetIntFromString_ShouldConvertStringToInt(string input, int expected)
    {
        var result = NumericHelper.GetIntFromString(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void GetIntFromString_InvalidInput_ShouldThrowArgumentException(string input)
    {
        Action act = () => NumericHelper.GetIntFromString(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("123.456", 123.456)]
    [InlineData("-123.456", -123.456)]
    public void GetDecimalFromString_ShouldConvertStringToDecimal(string input, decimal expected)
    {
        var result = NumericHelper.GetDecimalFromString(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void GetDecimalFromString_InvalidInput_ShouldThrowArgumentException(string input)
    {
        Action act = () => NumericHelper.GetDecimalFromString(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(123.456789, 2, 123.45)]
    [InlineData(123.456789, 0, 123)]
    public void Truncate_ShouldReturnValueWithSpecifiedPrecision(double value, int precision, double expected)
    {
        var result = value.Truncate(precision);
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData(double.NaN, false)]
    [InlineData(double.PositiveInfinity, false)]
    [InlineData(123.456, true)]
    public void IsValidNumericValue_ShouldReturnCorrectResult(double value, bool expected)
    {
        var result = NumericHelper.IsValidNumericValue(value);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("00123", true)]
    [InlineData("-123", false)]
    public void IsNaturalNumber_ShouldReturnCorrectResult(string input, bool expected)
    {
        var result = NumericHelper.IsNaturalNumber(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("0", true)]
    [InlineData("-123", false)]
    public void IsWholeNumber_ShouldReturnCorrectResult(string input, bool expected)
    {
        var result = NumericHelper.IsWholeNumber(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("-123", true)]
    [InlineData("abc", false)]
    public void IsInteger_ShouldReturnCorrectResult(string input, bool expected)
    {
        var result = NumericHelper.IsInteger(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123.45", true)]
    [InlineData("0.123", true)]
    [InlineData("-123.45", false)]
    public void IsPositiveNumber_ShouldReturnCorrectResult(string input, bool expected)
    {
        var result = NumericHelper.IsPositiveNumber(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123.45", true)]
    [InlineData("-123.45", true)]
    [InlineData("abc", false)]
    public void IsNumber_ShouldReturnCorrectResult(string input, bool expected)
    {
        var result = NumericHelper.IsNumber(input);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5.0, 1.0, 10.0, true)]
    [InlineData(0.0, 1.0, 10.0, false)]
    public void IsInRange_ShouldReturnCorrectResult(double value, double min, double max, bool expected)
    {
        var result = NumericHelper.IsInRange(value, min, max);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("invalid", 0.0)]
    public void GetDoubleOrDefault_ShouldReturnCorrectValue(string input, double expected)
    {
        var result = NumericHelper.GetDoubleOrDefault(input);
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData(123.45, "en-US", "$123.45")]
    [InlineData(123.45, "fr-FR", "123,45 €")]
    public void ToCurrency_ShouldReturnFormattedCurrencyString(double value, string culture, string expected)
    {
        var result = NumericHelper.ToCurrency(value, culture);
        result.Should().Be(expected);
    }
}

