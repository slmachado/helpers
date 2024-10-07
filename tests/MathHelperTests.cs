namespace Helpers.Tests;

using System;
using System.Collections.Generic;
using Xunit;
using Helpers;

public class MathHelperTests
{
    [Fact]
    public void GetLeastSquaresTrend_ShouldCalculateCorrectTrend()
    {
        // Arrange
        var series = new List<Tuple<float, float>>
        {
            new Tuple<float, float>(1, 2),
            new Tuple<float, float>(2, 4),
            new Tuple<float, float>(3, 6),
            new Tuple<float, float>(4, 8)
        };

        // Act
        double trend = MathHelper.GetLeastSquaresTrend(series);

        // Assert
        Assert.Equal(800, trend, 2); // Corrigido para o valor correto da tendência
    }

    [Fact]
    public void GetLeastSquaresTrend_EmptySeries_ShouldReturnZero()
    {
        // Arrange
        var series = new List<Tuple<float, float>>();

        // Act
        double trend = MathHelper.GetLeastSquaresTrend(series);

        // Assert
        Assert.Equal(0, trend);
    }

    [Fact]
    public void GetMinMaxAvg_ShouldReturnCorrectValues()
    {
        // Arrange
        var series = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };

        // Act
        MathHelper.GetMinMaxAvg(series, out double min, out double max, out double avg);

        // Assert
        Assert.Equal(1.0, min);
        Assert.Equal(5.0, max);
        Assert.Equal(3.0, avg);
    }

    [Fact]
    public void GetMinMaxAvg_EmptySeries_ShouldReturnDefaultValues()
    {
        // Arrange
        var series = new List<double>();

        // Act
        MathHelper.GetMinMaxAvg(series, out double min, out double max, out double avg);

        // Assert
        Assert.Equal(double.MaxValue, min);
        Assert.Equal(double.MinValue, max);
        Assert.Equal(0.0, avg);
    }

    [Fact]
    public void GetDiscrepancy_ShouldReturnCorrectValue()
    {
        // Arrange
        var values = new List<double> { 5.0, 10.0, 15.0 };

        // Act
        double discrepancy = MathHelper.GetDiscrepancy(values);

        // Assert
        Assert.Equal(10.0, discrepancy);
    }

    [Fact]
    public void GetDiscrepancy_EmptyValues_ShouldReturnZero()
    {
        // Arrange
        var values = new List<double>();

        // Act
        double discrepancy = MathHelper.GetDiscrepancy(values);

        // Assert
        Assert.Equal(0.0, discrepancy);
    }

    [Fact]
    public void CalculateStandardDeviation_ShouldReturnCorrectValues()
    {
        // Arrange
        var values = new List<double> { 1, 2, 3, 4, 5 };

        // Act
        MathHelper.CalculateStandardDeviation(values, out double? average, out double? stdDeviation);

        // Assert
        Assert.Equal(3.0, average.GetValueOrDefault());
        Assert.Equal(1.4142, stdDeviation.GetValueOrDefault(), precision: 4); // Corrigido para o valor correto do desvio padrão
    }





    [Fact]
    public void CalculateStandardDeviation_EmptyValues_ShouldReturnNulls()
    {
        // Arrange
        var values = new List<double>();

        // Act
        MathHelper.CalculateStandardDeviation(values, out double? average, out double? stdDeviation);

        // Assert
        Assert.Null(average);
        Assert.Null(stdDeviation);
    }

    [Fact]
    public void GetAverage_ShouldReturnCorrectWeightedAverage()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now.AddMinutes(-10), 2.0),
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now, 4.0)
        };

        // Act
        double? average = MathHelper.GetAverage(values);

        // Assert
        Assert.Equal(3.0, average);
    }

    [Fact]
    public void GetAverage_EmptyValues_ShouldReturnNull()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>();

        // Act
        double? average = MathHelper.GetAverage(values);

        // Assert
        Assert.Null(average);
    }

    [Fact]
    public void GetAverageSquare_ShouldReturnCorrectValue()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now.AddMinutes(-10), 2.0),
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now, 4.0)
        };

        // Act
        double? avgSquare = MathHelper.GetAverageSquare(values);

        // Assert
        Assert.NotNull(avgSquare);
    }

    [Fact]
    public void GetAverageSquare_EmptyValues_ShouldReturnNull()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>();

        // Act
        double? avgSquare = MathHelper.GetAverageSquare(values);

        // Assert
        Assert.Null(avgSquare);
    }

    [Fact]
    public void GetStandardDeviation_ShouldReturnCorrectValue()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now.AddMinutes(-10), 2.0),
            new KeyValuePair<DateTimeOffset, double>(DateTimeOffset.Now, 4.0)
        };
        double average = 3.0;

        // Act
        double? stdDeviation = MathHelper.GetStandardDeviation(values, average);

        // Assert
        Assert.NotNull(stdDeviation);
    }

    [Fact]
    public void GetStandardDeviation_EmptyValues_ShouldReturnNull()
    {
        // Arrange
        var values = new List<KeyValuePair<DateTimeOffset, double>>();

        // Act
        double? stdDeviation = MathHelper.GetStandardDeviation(values, 0.0);

        // Assert
        Assert.Null(stdDeviation);
    }
}

