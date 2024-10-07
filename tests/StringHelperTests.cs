using FluentAssertions;

namespace Helpers.Tests;
public class StringHelperTests
{
    [Fact]
    public void IsAlpha_ShouldReturnTrue_WhenStringIsOnlyAlphabetic()
    {
        // Arrange
        string input = "HelloWorld";

        // Act
        bool result = StringHelper.IsAlpha(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAlpha_ShouldReturnFalse_WhenStringContainsNonAlphabeticCharacters()
    {
        // Arrange
        string input = "Hello123";

        // Act
        bool result = StringHelper.IsAlpha(input);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAlphaNumeric_ShouldReturnTrue_WhenStringIsAlphanumeric()
    {
        // Arrange
        string input = "Hello123";

        // Act
        bool result = StringHelper.IsAlphaNumeric(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAlphaNumeric_ShouldReturnFalse_WhenStringContainsSpecialCharacters()
    {
        // Arrange
        string input = "Hello@123";

        // Act
        bool result = StringHelper.IsAlphaNumeric(input);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToDoubleNullable_ShouldReturnDouble_WhenStringIsNumeric()
    {
        // Arrange
        string input = "123.45";

        // Act
        double? result = input.ToDoubleNullable();

        // Assert
        result.Should().Be(123.45);
    }

    [Fact]
    public void ToDoubleNullable_ShouldReturnNull_WhenStringIsNotNumeric()
    {
        // Arrange
        string input = "NotANumber";

        // Act
        double? result = input.ToDoubleNullable();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToEnumerable_ShouldReturnLines_WhenStringHasMultipleLines()
    {
        // Arrange
        string input = "Line1\nLine2\nLine3";

        // Act
        IEnumerable<string> result = input.ToEnumerable();

        // Assert
        result.Should().BeEquivalentTo(new[] { "Line1", "Line2", "Line3" });
    }

    [Fact]
    public void TextAfter_ShouldReturnSubstring_WhenSearchStringIsFound()
    {
        // Arrange
        string input = "Hello, world!";
        string search = "Hello, ";

        // Act
        string? result = input.TextAfter(search);

        // Assert
        result.Should().Be("world!");
    }

    [Fact]
    public void TextAfter_ShouldReturnNull_WhenSearchStringIsNotFound()
    {
        // Arrange
        string input = "Hello, world!";
        string search = "Goodbye";

        // Act
        string? result = input.TextAfter(search);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetTextFromLine_ShouldReturnCorrectLine_WhenLineExists()
    {
        // Arrange
        string input = "Line1\nLine2\nLine3";

        // Act
        string? result = input.GetTextFromLine(2);

        // Assert
        result.Should().Be("Line2");
    }

    [Fact]
    public void GetTextFromLine_ShouldReturnNull_WhenLineDoesNotExist()
    {
        // Arrange
        string input = "Line1\nLine2";

        // Act
        string? result = input.GetTextFromLine(4);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetLineNumber_ShouldReturnCorrectLineNumber_WhenSubstringIsFound()
    {
        // Arrange
        string input = "Line1\nLine2\nLine3";

        // Act
        int result = input.GetLineNumber("Line2");

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void GetLineNumber_ShouldReturnMinusOne_WhenSubstringIsNotFound()
    {
        // Arrange
        string input = "Line1\nLine2\nLine3";

        // Act
        int result = input.GetLineNumber("Line4");

        // Assert
        result.Should().Be(-1);
    }

    [Fact]
    public void IsNumeric_ShouldReturnTrue_WhenStringIsOnlyDigits()
    {
        // Arrange
        string input = "123456";

        // Act
        bool result = StringHelper.IsNumeric(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNumeric_ShouldReturnFalse_WhenStringContainsNonDigitCharacters()
    {
        // Arrange
        string input = "123abc";

        // Act
        bool result = StringHelper.IsNumeric(input);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToTitleCase_ShouldReturnTitleCaseString_WhenStringIsLowercase()
    {
        // Arrange
        string input = "hello world";

        // Act
        string result = input.ToTitleCase();

        // Assert
        result.Should().Be("Hello World");
    }

    [Fact]
    public void ReverseString_ShouldReturnReversedString_WhenInputIsGiven()
    {
        // Arrange
        string input = "abcd";

        // Act
        string result = input.ReverseString();

        // Assert
        result.Should().Be("dcba");
    }

    [Fact]
    public void IsInteger_ShouldReturnTrue_WhenStringIsValidInteger()
    {
        // Arrange
        string input = "123";

        // Act
        bool result = StringHelper.IsInteger(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInteger_ShouldReturnFalse_WhenStringIsNotValidInteger()
    {
        // Arrange
        string input = "123.45";

        // Act
        bool result = StringHelper.IsInteger(input);

        // Assert
        result.Should().BeFalse();
    }
}
