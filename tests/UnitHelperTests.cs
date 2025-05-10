namespace Helpers.Tests;

using FluentAssertions;
using Helpers;
using Xunit;

public class UnitHelperTests
{
    [Theory]
    [InlineData(0, 273.15)]
    [InlineData(100, 373.15)]
    [InlineData(-273.15, 0)]
    public void ConvertDegreeToKelvin_ShouldConvertCelsiusToKelvin(double celsius, double expectedKelvin)
    {
        // Act
        double result = UnitHelper.ConvertDegreeToKelvin(celsius);

        // Assert
        result.Should().BeApproximately(expectedKelvin, 0.0001);
    }

    [Theory]
    [InlineData(0, -273.15)]
    [InlineData(373.15, 100)]
    [InlineData(273.15, 0)]
    public void ConvertKelvinToDegree_ShouldConvertKelvinToCelsius(double kelvin, double expectedCelsius)
    {
        // Act
        double result = UnitHelper.ConvertKelvinToDegree(kelvin);

        // Assert
        result.Should().BeApproximately(expectedCelsius, 0.0001);
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(100, 212)]
    [InlineData(-40, -40)]
    public void ConvertCelsiusToFahrenheit_ShouldConvertCelsiusToFahrenheit(double celsius, double expectedFahrenheit)
    {
        // Act
        double result = UnitHelper.ConvertCelsiusToFahrenheit(celsius);

        // Assert
        result.Should().BeApproximately(expectedFahrenheit, 0.0001);
    }

    [Theory]
    [InlineData(32, 0)]
    [InlineData(212, 100)]
    [InlineData(-40, -40)]
    public void ConvertFahrenheitToCelsius_ShouldConvertFahrenheitToCelsius(double fahrenheit, double expectedCelsius)
    {
        // Act
        double result = UnitHelper.ConvertFahrenheitToCelsius(fahrenheit);

        // Assert
        result.Should().BeApproximately(expectedCelsius, 0.0001);
    }

    [Theory]
    [InlineData(1, 1.60934)]
    [InlineData(10, 16.0934)]
    [InlineData(0, 0)]
    public void ConvertMilesToKilometers_ShouldConvertMilesToKilometers(double miles, double expectedKilometers)
    {
        // Act
        double result = UnitHelper.ConvertMilesToKilometers(miles);

        // Assert
        result.Should().BeApproximately(expectedKilometers, 0.0001);
    }

    [Theory]
    [InlineData(1.60934, 1)]
    [InlineData(16.0934, 10)]
    [InlineData(0, 0)]
    public void ConvertKilometersToMiles_ShouldConvertKilometersToMiles(double kilometers, double expectedMiles)
    {
        // Act
        double result = UnitHelper.ConvertKilometersToMiles(kilometers);

        // Assert
        result.Should().BeApproximately(expectedMiles, 0.0001);
    }

    [Theory]
    [InlineData(1, 0.453592)]
    [InlineData(10, 4.53592)]
    [InlineData(0, 0)]
    public void ConvertPoundsToKilograms_ShouldConvertPoundsToKilograms(double pounds, double expectedKilograms)
    {
        // Act
        double result = UnitHelper.ConvertPoundsToKilograms(pounds);

        // Assert
        result.Should().BeApproximately(expectedKilograms, 0.0001);
    }

    [Theory]
    [InlineData(0.453592, 1)]
    [InlineData(4.53592, 10)]
    [InlineData(0, 0)]
    public void ConvertKilogramsToPounds_ShouldConvertKilogramsToPounds(double kilograms, double expectedPounds)
    {
        // Act
        double result = UnitHelper.ConvertKilogramsToPounds(kilograms);

        // Assert
        result.Should().BeApproximately(expectedPounds, 0.0001);
    }

    [Theory]
    [InlineData(1, 0.264172)]
    [InlineData(10, 2.64172)]
    [InlineData(0, 0)]
    public void ConvertLitersToGallons_ShouldConvertLitersToGallons(double liters, double expectedGallons)
    {
        // Act
        double result = UnitHelper.ConvertLitersToGallons(liters);

        // Assert
        result.Should().BeApproximately(expectedGallons, 0.0001);
    }

    [Theory]
    [InlineData(0.264172, 1)]
    [InlineData(2.64172, 10)]
    [InlineData(0, 0)]
    public void ConvertGallonsToLiters_ShouldConvertGallonsToLiters(double gallons, double expectedLiters)
    {
        // Act
        double result = UnitHelper.ConvertGallonsToLiters(gallons);

        // Assert
        result.Should().BeApproximately(expectedLiters, 0.0001);
    }

    [Theory]
    [InlineData(1, 2.54)]
    [InlineData(10, 25.4)]
    [InlineData(0, 0)]
    public void ConvertInchesToCentimeters_ShouldConvertInchesToCentimeters(double inches, double expectedCentimeters)
    {
        // Act
        double result = UnitHelper.ConvertInchesToCentimeters(inches);

        // Assert
        result.Should().BeApproximately(expectedCentimeters, 0.0001);
    }

    [Theory]
    [InlineData(2.54, 1)]
    [InlineData(25.4, 10)]
    [InlineData(0, 0)]
    public void ConvertCentimetersToInches_ShouldConvertCentimetersToInches(double centimeters, double expectedInches)
    {
        // Act
        double result = UnitHelper.ConvertCentimetersToInches(centimeters);

        // Assert
        result.Should().BeApproximately(expectedInches, 0.0001);
    }
}

