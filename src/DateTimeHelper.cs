using System.Globalization;

namespace Helpers;

/// <summary>
/// DateTime handling tools
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Gets the minimum DateTime value.
    /// </summary>
    public static DateTime DateTimeMin => new DateTime(1931, 1, 1);

    /// <summary>
    /// Returns a formatted DateTime (UTC) based on the provided format.
    /// </summary>
    /// <param name="dateTime">The dateTime string to be parsed.</param>
    /// <param name="dateTimeFormat">The format of the dateTime string.</param>
    /// <returns>A DateTimeOffset parsed from the string.</returns>
    public static DateTimeOffset GetFormattedDateTime(string dateTime, string dateTimeFormat)
    {
        if (string.IsNullOrEmpty(dateTime))
        {
            throw new ArgumentException("Date time cannot be null or empty", nameof(dateTime));
        }

        try
        {
            return string.IsNullOrEmpty(dateTimeFormat)
                ? DateTimeOffset.Parse(dateTime, CultureInfo.InvariantCulture)
                : DateTimeOffset.ParseExact(dateTime, dateTimeFormat, CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid date format provided.", ex);
        }
    }

    /// <summary>
    /// Calculates the age in years from a given date.
    /// </summary>
    /// <param name="date">The date from which to calculate the age.</param>
    /// <returns>The age in years.</returns>
    public static double GetAgeInYear(DateTime date)
    {
        return (DateTime.Now - date).TotalDays / 365;
    }

    /// <summary>
    /// Gets the number of the week for the specified date, based on the current culture.
    /// </summary>
    /// <param name="date">The date to determine the week number.</param>
    /// <returns>The week number of the year.</returns>
    public static int GetWeekNumber(this DateTime date)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var calendar = cultureInfo.Calendar;
        var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

        return calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
    }

    /// <summary>
    /// Gets the first day of the week for the given date, based on the current culture.
    /// </summary>
    /// <param name="date">The date to find the first day of the week.</param>
    /// <returns>The first day of the week for the specified date.</returns>
    public static DateTime GetFirstDayOfWeek(this DateTime date)
    {
        var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        while (date.DayOfWeek != firstDayOfWeek)
        {
            date = date.AddDays(-1);
        }

        return date;
    }

    /// <summary>
    /// Returns a collection of dates between the start and end date, with the specified interval.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="delta">The interval between dates.</param>
    /// <returns>A collection of DateTimeOffset values between the start and end date.</returns>
    public static ICollection<DateTimeOffset> GetDatesInRange(DateTimeOffset startDate, DateTimeOffset endDate, TimeSpan delta)
    {
        if (startDate > endDate)
        {
            return new List<DateTimeOffset>();
        }

        var dates = new List<DateTimeOffset>();
        while (startDate <= endDate)
        {
            dates.Add(startDate);
            startDate = startDate.Add(delta);
        }

        return dates;
    }

    /// <summary>
    /// Converts the given date to the specified time zone.
    /// </summary>
    /// <param name="dateTime">The date to be converted.</param>
    /// <param name="timeZone">The target time zone.</param>
    /// <returns>A DateTimeOffset representing the date in the specified time zone.</returns>
    public static DateTimeOffset ConvertForTimeZone(this DateTimeOffset dateTime, TimeZoneInfo timeZone)
    {
        if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
        return dateTime.ToUniversalTime().ToOffset(timeZone.GetUtcOffset(dateTime));
    }

    /// <summary>
    /// Gets the quarter that corresponds to the specified month.
    /// </summary>
    /// <param name="month">The month to determine the quarter.</param>
    /// <returns>The quarter number (1-4).</returns>
    public static int GetQuarter(int month)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        return (int)Math.Ceiling(month / 3.0);
    }


    /// <summary>
    /// Returns dates that match a given frequency within a collection.
    /// </summary>
    /// <param name="dates">The collection of dates.</param>
    /// <param name="frequency">The frequency to match.</param>
    /// <returns>A collection of dates matching the frequency.</returns>
    public static IEnumerable<DateTimeOffset> GetDatesInFrequency(this IEnumerable<DateTimeOffset> dates, TimeSpan frequency)
    {
        if (dates == null) throw new ArgumentNullException(nameof(dates));
        if (!dates.Any()) return Enumerable.Empty<DateTimeOffset>();

        var goodDate = GetFirstGoodDateInFrequency(dates, frequency);
        if (goodDate == null) return Enumerable.Empty<DateTimeOffset>();

        return dates.Where(current => Math.Abs((goodDate.Value - current).TotalMinutes % frequency.TotalMinutes) < 0.000001);
    }


    /// <summary>
    /// Returns the first date that matches the frequency criteria from a given collection.
    /// </summary>
    /// <param name="dates">The collection of dates to search through.</param>
    /// <param name="frequency">The frequency to match.</param>
    /// <returns>The first date that matches the frequency, or null if none match.</returns>
    private static DateTimeOffset? GetFirstGoodDateInFrequency(IEnumerable<DateTimeOffset> dates, TimeSpan frequency)
    {
        foreach (var current in dates)
        {
            foreach (var nextDate in dates)
            {
                if (nextDate > current && ((nextDate - current).TotalMinutes % frequency.TotalMinutes).NearlyZero())
                {
                    return current;
                }
            }
        }

        return null;
    }


    /// <summary>
    /// Gets a list of dates separated by one month between the start and end date.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>A collection of DateTimeOffset values, one per month between the start and end date.</returns>
    public static IEnumerable<DateTimeOffset> GetDatesEachMonth(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return new List<DateTimeOffset>();
        }

        var dates = new List<DateTimeOffset>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            dates.Add(new DateTimeOffset(currentDate, TimeSpan.Zero));
            currentDate = currentDate.AddMonths(1);
        }

        return dates;
    }

}

