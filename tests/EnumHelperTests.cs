using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;


namespace Helpers.Tests;


public class EnumHelperTests
{
    public enum SampleEnum
    {
        FirstValue = 1,
        SecondValue = 2,
        [System.ComponentModel.Description("Third Value Description")]
        ThirdValue = 3,
        FourthValue = 4
    }


    [Fact]
    public void GetList_ShouldReturnAllEnumValues()
    {
        // Act
        var result = EnumHelper.GetList<SampleEnum>();

        // Assert
        result.Should().Contain(new[] { SampleEnum.FirstValue, SampleEnum.SecondValue, SampleEnum.ThirdValue, SampleEnum.FourthValue });
    }


    [Theory]
    [InlineData(1, SampleEnum.FirstValue)]
    [InlineData(2, SampleEnum.SecondValue)]
    [InlineData(3, SampleEnum.ThirdValue)]
    [InlineData(5, SampleEnum.FirstValue)] // Valor inválido retorna o primeiro valor
    public void ParseInt_ShouldReturnCorrectEnumValue(int input, SampleEnum expected)
    {
        // Act
        var result = EnumHelper.Parse<SampleEnum>(input);

        // Assert
        result.Should().Be(expected);
    }


    [Theory]
    [InlineData("FirstValue", SampleEnum.FirstValue)]
    [InlineData("secondvalue", SampleEnum.SecondValue)] // Case insensitive
    [InlineData("InvalidValue", SampleEnum.FirstValue)]  // Valor inválido retorna o primeiro valor
    public void ParseString_ShouldReturnCorrectEnumValue(string input, SampleEnum expected)
    {
        // Act
        var result = EnumHelper.Parse<SampleEnum>(input);

        // Assert
        result.Should().Be(expected);
    }


    [Theory]
    [InlineData(10, false, SampleEnum.FirstValue)]  // Valor inválido retorna o primeiro valor
    public void TryParseInt_ShouldReturnCorrectEnumValue(int input, bool expectedSuccess, SampleEnum expected)
    {
        // Act
        var success = EnumHelper.TryParse(input, out SampleEnum result);

        // Assert
        success.Should().Be(expectedSuccess);
        result.Should().Be(expected);
    }


    [Theory]
    [InlineData("NonExistentValue", false, SampleEnum.FirstValue)] // Valor inválido retorna o primeiro valor
    public void TryParseString_ShouldReturnCorrectEnumValue(string input, bool expectedSuccess, SampleEnum expected)
    {
        // Act
        var success = EnumHelper.TryParse(input, out SampleEnum result);

        // Assert
        success.Should().Be(expectedSuccess);
        result.Should().Be(expected);
    }

    [Fact]
    public void GetAttributeOfType_ShouldReturnDescriptionAttribute()
    {
        // Arrange
        var enumValue = SampleEnum.ThirdValue;

        // Act
        var attribute = enumValue.GetAttributeOfType<System.ComponentModel.DescriptionAttribute>();

        // Assert
        attribute.Should().NotBeNull();
        attribute!.Description.Should().Be("Third Value Description");
    }

    [Fact]
    public void GetDescription_ShouldReturnCorrectDescription()
    {
        // Arrange
        var enumValue = SampleEnum.ThirdValue;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        description.Should().Be("Third Value Description");
    }

    [Fact]
    public void GetDescription_ShouldReturnEnumNameIfNoDescription()
    {
        // Arrange
        var enumValue = SampleEnum.FirstValue;

        // Act
        var description = enumValue.GetDescription();

        // Assert
        description.Should().Be("FirstValue");
    }

    [Fact]
    public void ToList_ShouldReturnAllEnumValuesWithNamesAndIntegers()
    {
        // Act
        var result = EnumHelper.ToList<SampleEnum>();

        // Assert
        result.Should().Contain(new KeyValuePair<string, int>("FirstValue", 1));
        result.Should().Contain(new KeyValuePair<string, int>("SecondValue", 2));
        result.Should().Contain(new KeyValuePair<string, int>("ThirdValue", 3));
        result.Should().Contain(new KeyValuePair<string, int>("FourthValue", 4));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(4, true)]
    [InlineData(10, false)]
    public void IsDefinedInt_ShouldReturnIfValueIsDefined(int value, bool expected)
    {
        // Act
        var result = EnumHelper.IsDefined<SampleEnum>(value);

        // Assert
        result.Should().Be(expected);
    }


    [Theory]
    [InlineData("FirstValue", true)]
    [InlineData("fourthvalue", true)]  // Agora deve passar, pois a comparação é case-insensitive
    [InlineData("InvalidValue", false)]
    public void IsDefinedString_ShouldReturnIfValueIsDefined(string value, bool expected)
    {
        // Act
        var result = EnumHelper.IsDefined<SampleEnum>(value);

        // Assert
        result.Should().Be(expected);
    }


    [Fact]
    public void GetNames_ShouldReturnAllEnumNames()
    {
        // Act
        var result = EnumHelper.GetNames<SampleEnum>();

        // Assert
        result.Should().Contain(new[] { "FirstValue", "SecondValue", "ThirdValue", "FourthValue" });
    }

    [Fact]
    public void GetValues_ShouldReturnAllEnumValues()
    {
        // Act
        var result = EnumHelper.GetValues<SampleEnum>();

        // Assert
        result.Should().Contain(new[] { 1, 2, 3, 4 });
    }

    [Fact]
    public void GetDefaultValue_ShouldReturnFirstValueOfEnum()
    {
        // Act
        var result = EnumHelper.GetDefaultValue<SampleEnum>();

        // Assert
        result.Should().Be(SampleEnum.FirstValue);
    }

    [Fact]
    public void ConvertToDescriptionDictionary_ShouldReturnCorrectMapping()
    {
        // Act
        var result = EnumHelper.ConvertToDescriptionDictionary<SampleEnum>();

        // Assert
        result[1].Should().Be("FirstValue");
        result[2].Should().Be("SecondValue");
        result[3].Should().Be("Third Value Description");
        result[4].Should().Be("FourthValue");
    }
}
