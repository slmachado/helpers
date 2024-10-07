namespace Helpers.Tests;

public class NameOfHelperTests
{
    private class SampleClass
    {
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
    }

    [Fact]
    public void GetPropertyName_ShouldReturnCorrectPropertyName()
    {
        // Arrange
        var sample = new SampleClass();

        // Act
        string propertyName = sample.GetPropertyName(x => x.Property1);

        // Assert
        Assert.Equal("Property1", propertyName);
    }

    [Fact]
    public void GetPropertyName_ShouldThrowArgumentException_ForNonMemberExpression()
    {
        // Arrange
        var sample = new SampleClass();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sample.GetPropertyName(x => x.Property2 + 1));
    }
}
