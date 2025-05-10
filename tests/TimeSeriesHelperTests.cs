namespace Helpers.Tests;

using FluentAssertions;
using Helpers;
using System;
using System.Collections.Generic;
using Xunit;

public class TimeseriesHelperTests
{
    [Fact]
    public void GetPeriodeSpansUnderThreshold_ShouldReturnSpansUnderThreshold()
    {
        // Arrange
        var timeseries = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 1, 0, 0, 0, TimeSpan.Zero), 10),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), 5),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero), 4),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 4, 0, 0, 0, TimeSpan.Zero), 8),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 5, 0, 0, 0, TimeSpan.Zero), 12)
        };
        double threshold = 6;

        // Act
        var result = timeseries.GetPeriodeSpansUnderThreshold(threshold);

        // Assert
        result.Should().BeEquivalentTo(new List<Tuple<DateTimeOffset, DateTimeOffset>>
        {
            Tuple.Create(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero))
        }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetPeriodeSpansUnderThreshold_ShouldReturnEmpty_WhenNoValuesUnderThreshold()
    {
        // Arrange
        var timeseries = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 1, 0, 0, 0, TimeSpan.Zero), 10),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), 12),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero), 15)
        };
        double threshold = 6;

        // Act
        var result = timeseries.GetPeriodeSpansUnderThreshold(threshold);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ExcludeSpan_ShouldExcludeSpecifiedSpan()
    {
        // Arrange
        var timeseries = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 1, 0, 0, 0, TimeSpan.Zero), 10),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), 5),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero), 4),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 4, 0, 0, 0, TimeSpan.Zero), 8),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 5, 0, 0, 0, TimeSpan.Zero), 12)
        };

        var spansToExclude = new List<Tuple<DateTimeOffset, DateTimeOffset>>
        {
            Tuple.Create(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero))
        };

        // Act
        var result = timeseries.ExcludeSpan(spansToExclude);

        // Assert
        result.Should().BeEquivalentTo(new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 1, 0, 0, 0, TimeSpan.Zero), 10),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 4, 0, 0, 0, TimeSpan.Zero), 8),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 5, 0, 0, 0, TimeSpan.Zero), 12)
        }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ExcludeSpan_ShouldReturnOriginalSeries_WhenNoSpansToExclude()
    {
        // Arrange
        var timeseries = new List<KeyValuePair<DateTimeOffset, double>>
        {
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 1, 0, 0, 0, TimeSpan.Zero), 10),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 2, 0, 0, 0, TimeSpan.Zero), 5),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 3, 0, 0, 0, TimeSpan.Zero), 4),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 4, 0, 0, 0, TimeSpan.Zero), 8),
            new KeyValuePair<DateTimeOffset, double>(new DateTimeOffset(2024, 10, 5, 0, 0, 0, TimeSpan.Zero), 12)
        };

        var spansToExclude = new List<Tuple<DateTimeOffset, DateTimeOffset>>();

        // Act
        var result = timeseries.ExcludeSpan(spansToExclude);

        // Assert
        result.Should().BeEquivalentTo(timeseries, options => options.WithStrictOrdering());
    }
}