using System.Globalization;
using System.Text.RegularExpressions;

namespace Helpers;

/// <summary>
/// Provides various numeric helper methods for parsing, validation, and formatting.
/// </summary>
public static class NumericHelper
{
    // Reusable regex patterns, optimized for performance
    private static readonly Regex NotNaturalPattern = new("[^0-9]", RegexOptions.Compiled);
    private static readonly Regex NaturalPattern = new("0*[1-9][0-9]*", RegexOptions.Compiled);
    private static readonly Regex NotWholePattern = new("[^0-9]", RegexOptions.Compiled);
    private static readonly Regex NotIntPattern = new("[^0-9-]", RegexOptions.Compiled);
    private static readonly Regex IntPattern = new("^-[0-9]+$|^[0-9]+$", RegexOptions.Compiled);
    private static readonly Regex NotPositivePattern = new("[^0-9.]", RegexOptions.Compiled);
    private static readonly Regex PositivePattern = new("^[.][0-9]+$|[0-9]*[.]*[0-9]+$", RegexOptions.Compiled);
    private static readonly Regex TwoDotPattern = new("[0-9]*[.][0-9]*[.][0-9]*", RegexOptions.Compiled);
    private static readonly Regex NotNumberPattern = new("[^0-9.-]", RegexOptions.Compiled);
    private static readonly Regex TwoMinusPattern = new("[0-9]*[-][0-9]*[-][0-9]*", RegexOptions.Compiled);
    private static readonly Regex NumberPattern = new(@"^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$|^([-]|[0-9])[0-9]*$", RegexOptions.Compiled);

    /// <summary>
    /// Converts a string to a double value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <returns>The corresponding double value.</returns>
    /// <exception cref="ArgumentException">If the string is not a valid double value.</exception>
    public static double GetDoubleFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return doubleValue;
        }

        throw new ArgumentException("The value is not a valid double");
    }

    /// <summary>
    /// Attempts to convert a string to a double value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <param name="doubleValue">The converted double value, if successful.</param>
    /// <returns>True if the conversion is successful, otherwise false.</returns>
    public static bool TryGetDoubleFromString(string value, out double doubleValue)
    {
        return double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out doubleValue);
    }

    /// <summary>
    /// Converts a string to a float value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <returns>The corresponding float value.</returns>
    /// <exception cref="ArgumentException">If the string is not a valid float value.</exception>
    public static float GetFloatFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (float.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue))
        {
            return floatValue;
        }

        throw new ArgumentException("The value is not a valid float");
    }

    /// <summary>
    /// Converts a string to an integer value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <returns>The corresponding integer value.</returns>
    /// <exception cref="ArgumentException">If the string is not a valid integer value.</exception>
    public static int GetIntFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (int.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        throw new ArgumentException("The value is not a valid integer");
    }

    /// <summary>
    /// Attempts to convert a string to an integer value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <param name="intValue">The converted integer value, if successful.</param>
    /// <returns>True if the conversion is successful, otherwise false.</returns>
    public static bool TryGetIntFromString(string value, out int intValue)
    {
        return int.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out intValue);
    }

    /// <summary>
    /// Converts a string to a decimal value.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <returns>The corresponding decimal value.</returns>
    /// <exception cref="ArgumentException">If the string is not a valid decimal value.</exception>
    public static decimal GetDecimalFromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (decimal.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
        {
            return decimalValue;
        }

        throw new ArgumentException("The value is not a valid decimal");
    }

    /// <summary>
    /// Truncates a double value to a specified number of decimal places.
    /// </summary>
    /// <param name="value">The value to be truncated.</param>
    /// <param name="precision">The number of decimal places.</param>
    /// <returns>The truncated value.</returns>
    public static double Truncate(this double value, int precision)
    {
        if (precision < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(precision), "Precision must be non-negative.");
        }

        double step = Math.Pow(10, precision);
        double tmp = Math.Truncate(step * value);
        return tmp / step;
    }

    /// <summary>
    /// Checks if the numeric value is valid (not NaN or infinity).
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns>True if the value is valid, otherwise false.</returns>
    public static bool IsValidNumericValue(double value)
    {
        return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    /// <summary>
    /// Checks if the string represents a natural number (positive and without fractions).
    /// </summary>
    /// <param name="strNumber">The string to be checked.</param>
    /// <returns>True if it is a natural number, otherwise false.</returns>
    public static bool IsNaturalNumber(string strNumber)
    {
        return !NotNaturalPattern.IsMatch(strNumber) && NaturalPattern.IsMatch(strNumber);
    }

    /// <summary>
    /// Checks if the string represents a non-negative integer.
    /// </summary>
    /// <param name="strNumber">The string to be checked.</param>
    /// <returns>True if it is a non-negative integer, otherwise false.</returns>
    public static bool IsWholeNumber(string strNumber)
    {
        return !NotWholePattern.IsMatch(strNumber);
    }

    /// <summary>
    /// Checks if the string represents an integer (positive or negative).
    /// </summary>
    /// <param name="strNumber">The string to be checked.</param>
    /// <returns>True if it is an integer, otherwise false.</returns>
    public static bool IsInteger(string strNumber)
    {
        return !NotIntPattern.IsMatch(strNumber) && IntPattern.IsMatch(strNumber);
    }

    /// <summary>
    /// Checks if the string represents a positive number (integer or real).
    /// </summary>
    /// <param name="strNumber">The string to be checked.</param>
    /// <returns>True if it is a positive number, otherwise false.</returns>
    public static bool IsPositiveNumber(string strNumber)
    {
        return !NotPositivePattern.IsMatch(strNumber) &&
               PositivePattern.IsMatch(strNumber) &&
               !TwoDotPattern.IsMatch(strNumber);
    }

    /// <summary>
    /// Checks if the string represents a valid number.
    /// </summary>
    /// <param name="strNumber">The string to be checked.</param>
    /// <returns>True if it is a valid number, otherwise false.</returns>
    public static bool IsNumber(string strNumber)
    {
        return !NotNumberPattern.IsMatch(strNumber) &&
               !TwoDotPattern.IsMatch(strNumber) &&
               !TwoMinusPattern.IsMatch(strNumber) &&
               NumberPattern.IsMatch(strNumber);
    }

    /// <summary>
    /// Checks if a value is within a specific range.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>True if the value is within the range, otherwise false.</returns>
    public static bool IsInRange(double value, double min, double max)
    {
        return value >= min && value <= max;
    }

    /// <summary>
    /// Attempts to get a double value from a string, returning a default value if it fails.
    /// </summary>
    /// <param name="value">The string to be converted.</param>
    /// <param name="defaultValue">The default value to return in case of failure.</param>
    /// <returns>The converted double value or the default value.</returns>
    public static double GetDoubleOrDefault(string value, double defaultValue = 0.0)
    {
        return TryGetDoubleFromString(value, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Converts a double value to a currency representation in a specific culture.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <param name="culture">The culture to be used for formatting (default is "en-US").</param>
    /// <returns>The value represented as a formatted currency string.</returns>
    public static string ToCurrency(double value, string culture = "en-US")
    {
        return value.ToString("C", new CultureInfo(culture));
    }
}
