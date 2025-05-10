namespace Helpers;

public static class BooleanHelper
{
    /// <summary>
    /// Converts a string value to an integer representation of a boolean (1 for true, 0 for false).
    /// Throws an exception if the value cannot be parsed.
    /// </summary>
    /// <param name="value">The input string to convert.</param>
    /// <returns>An integer representing the boolean value (0 or 1).</returns>
    /// <exception cref="ArgumentException">Thrown when the input value is not a valid boolean.</exception>
    public static int GetBooleanIntValueFromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The input value cannot be null or empty", nameof(value));
        }

        if (TryParseBooleanIntValueFromString(value, out var intValue))
        {
            return intValue;
        }

        throw new ArgumentException("The input value is not a valid boolean representation");
    }

    /// <summary>
    /// Attempts to parse a string value into an integer representation of a boolean (1 for true, 0 for false).
    /// </summary>
    /// <param name="value">The input string to parse.</param>
    /// <param name="boolValue">The output integer representing the boolean value (0 or 1).</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public static bool TryParseBooleanIntValueFromString(string value, out int boolValue)
    {
        boolValue = 0;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (NumericHelper.TryGetIntFromString(value, out var parsedValue) && (parsedValue == 0 || parsedValue == 1))
        {
            boolValue = parsedValue;
            return true;
        }

        if (bool.TryParse(value, out var boolParsed))
        {
            boolValue = boolParsed ? 1 : 0;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Converts a string value to a boolean. Throws an exception if the value cannot be parsed.
    /// </summary>
    /// <param name="value">The input string to convert.</param>
    /// <returns>A boolean representation of the input value.</returns>
    /// <exception cref="ArgumentException">Thrown when the input value is not a valid boolean representation.</exception>
    public static bool GetBooleanFromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The input value cannot be null or empty", nameof(value));
        }

        if (NumericHelper.TryGetIntFromString(value, out var intValue))
        {
            if (intValue == 0) return false;
            if (intValue == 1) return true;
        }

        if (bool.TryParse(value, out var boolValue))
        {
            return boolValue;
        }

        throw new ArgumentException("The input value is not a valid boolean representation");
    }

    /// <summary>
    /// Attempts to parse a string value into a boolean.
    /// </summary>
    /// <param name="value">The input string to parse.</param>
    /// <param name="boolValue">The output boolean value.</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public static bool TryParseBooleanFromString(string value, out bool boolValue)
    {
        boolValue = false;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (NumericHelper.TryGetIntFromString(value, out var intValue))
        {
            if (intValue == 0)
            {
                boolValue = false;
                return true;
            }

            if (intValue == 1)
            {
                boolValue = true;
                return true;
            }
        }

        return bool.TryParse(value, out boolValue);
    }

    /// <summary>
    /// Converts a string value to a boolean based on custom true/false string representations.
    /// Throws an exception if the value cannot be parsed.
    /// </summary>
    /// <param name="value">The input string to convert.</param>
    /// <param name="trueString">The string representation of true.</param>
    /// <param name="falseString">The string representation of false.</param>
    /// <returns>A boolean representation of the input value.</returns>
    /// <exception cref="ArgumentException">Thrown when the input value is not a valid boolean representation.</exception>
    public static bool GetBooleanFromString(string value, string trueString, string falseString)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The input value cannot be null or empty", nameof(value));
        }

        if (value.Equals(trueString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (value.Equals(falseString, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        throw new ArgumentException("The input value is not a valid boolean representation");
    }

    /// <summary>
    /// Attempts to parse a string value into a boolean based on custom true/false string representations.
    /// </summary>
    /// <param name="value">The input string to parse.</param>
    /// <param name="trueString">The string representation of true.</param>
    /// <param name="falseString">The string representation of false.</param>
    /// <param name="boolValue">The output boolean value.</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public static bool TryParseBooleanFromString(string value, string trueString, string falseString, out bool boolValue)
    {
        boolValue = false;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (value.Equals(trueString, StringComparison.OrdinalIgnoreCase))
        {
            boolValue = true;
            return true;
        }

        if (value.Equals(falseString, StringComparison.OrdinalIgnoreCase))
        {
            boolValue = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Formats a boolean value to a string representation based on input parameters.
    /// </summary>
    /// <param name="value">The boolean value to format.</param>
    /// <param name="trueString">The string representation of true.</param>
    /// <param name="falseString">The string representation of false.</param>
    /// <returns>A string representation of the boolean value.</returns>
    public static string FormatBooleanToString(bool value, string trueString, string falseString)
    {
        return value ? trueString : falseString;
    }
}