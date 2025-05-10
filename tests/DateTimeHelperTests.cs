using FluentAssertions;

namespace Helpers.Tests;

public class DateTimeHelperTests
{
    [Fact]
    public void DateTimeMin_ShouldReturnCorrectMinimumDate()
    {
        // Act
        var result = DateTimeHelper.DateTimeMin;

        // Assert
        result.Should().Be(new DateTime(1931, 1, 1));
    }

    [Fact]
    public void GetFormattedDateTime_ShouldReturnFormattedDate_WhenValidInput()
    {
        // Arrange
        string date = "2024-01-01";
        string format = "yyyy-MM-dd";

        // Act
        var result = DateTimeHelper.GetFormattedDateTime(date, format);

        // Assert
        result.Should().Be(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void GetFormattedDateTime_ShouldThrowFormatException_WhenInvalidDateFormat()
    {
        // Arrange
        string date = "01-01-2024";
        string format = "yyyy-MM-dd";

        // Act
        Action act = () => DateTimeHelper.GetFormattedDateTime(date, format);

        // Assert
        act.Should().Throw<FormatException>().WithMessage("Invalid date format provided.*");
    }

    [Fact]
    public void GetAgeInYear_ShouldReturnCorrectAge()
    {
        // Arrange
        var birthDate = DateTime.Now.AddYears(-25);

        // Act
        var age = DateTimeHelper.GetAgeInYear(birthDate);

        // Assert
        age.Should().BeApproximately(25, 0.1);
    }

    [Fact]
    public void GetWeekNumber_ShouldReturnCorrectWeekNumber()
    {
        // Arrange
        var date = new DateTime(2024, 1, 3); // Wednesday of the 1st week

        // Act
        var weekNumber = date.GetWeekNumber();

        // Assert
        weekNumber.Should().Be(1);
    }

    [Fact]
    public void GetFirstDayOfWeek_ShouldReturnCorrectFirstDay()
    {
        // Arrange
        var date = new DateTime(2024, 1, 3); // Wednesday

        // Act
        var firstDayOfWeek = date.GetFirstDayOfWeek();

        // Assert
        firstDayOfWeek.Should().Be(new DateTime(2023, 12, 31)); // Assuming the first day of the week is Sunday
    }

    [Fact]
    public void GetDatesInRange_ShouldReturnCorrectDates()
    {
        // Arrange
        var startDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(2024, 1, 3, 0, 0, 0, TimeSpan.Zero);
        var delta = TimeSpan.FromDays(1);

        // Act
        var dates = DateTimeHelper.GetDatesInRange(startDate, endDate, delta).ToList();

        // Assert
        dates.Should().HaveCount(3);
        dates[0].Should().Be(startDate);
        dates[2].Should().Be(endDate);
    }

    [Fact]
    public void ConvertForTimeZone_ShouldReturnCorrectConvertedDate()
    {
        // Arrange
        var dateTime = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        // Act
        var convertedDate = dateTime.ConvertForTimeZone(timeZone);

        // Assert
        convertedDate.Offset.Should().Be(timeZone.GetUtcOffset(dateTime));
    }

    [Fact]
    public void GetQuarter_ShouldReturnCorrectQuarter()
    {
        // Arrange
        int month = 5; // May

        // Act
        var quarter = DateTimeHelper.GetQuarter(month);

        // Assert
        quarter.Should().Be(2);
    }

    [Fact]
    public void GetDatesInFrequency_ShouldReturnCorrectDates()
    {
        // Arrange
        var dates = new List<DateTimeOffset>
        {
            new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero)
        };
        var frequency = TimeSpan.FromHours(12);

        // Act
        var result = dates.GetDatesInFrequency(frequency).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[1].Should().Be(dates[1]);
    }

    [Fact]
    public void GetDatesEachMonth_ShouldReturnDatesSeparatedByOneMonth()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 4, 1);

        // Act
        var dates = DateTimeHelper.GetDatesEachMonth(startDate, endDate).ToList();

        // Assert
        dates.Should().HaveCount(4);
        dates[0].Date.Should().Be(new DateTime(2024, 1, 1));
        dates[3].Date.Should().Be(new DateTime(2024, 4, 1));
    }
}
