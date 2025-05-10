namespace Helpers;

/// <summary>
/// Helper class for checking floating point equality and managing imprecision of floating-point types.
/// </summary>
public static class FloatingNumberHelper
{
    private const double Epsilon = 0.00001d; // Aumentando o epsilon para maior tolerância

    /// <summary>
    /// Checks if two double values are nearly equal, allowing for a specified tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The value to compare against.</param>
    /// <param name="epsilon">The acceptable difference between the two values.</param>
    /// <returns>Returns true if the two values are nearly equal within the specified tolerance.</returns>
    public static bool NearlyEqual(this double value, double compareTo, double epsilon) =>
        Math.Abs(value - compareTo) <= epsilon;

    /// <summary>
    /// Checks if two double values are nearly equal, using the default tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The value to compare against.</param>
    /// <returns>Returns true if the two values are nearly equal within the default tolerance.</returns>
    public static bool NearlyEqual(this double value, double compareTo) => NearlyEqual(value, compareTo, Epsilon);

    /// <summary>
    /// Checks if two nullable double values are nearly equal, allowing for default tolerance.
    /// </summary>
    /// <param name="value">The first nullable value to compare.</param>
    /// <param name="compareTo">The nullable value to compare against.</param>
    /// <returns>Returns true if both values are null or are nearly equal within the default tolerance.</returns>
    public static bool NearlyEqual(this double? value, double? compareTo) =>
        value.HasValue && compareTo.HasValue ? NearlyEqual(value.Value, compareTo.Value, Epsilon) : value == compareTo;

    /// <summary>
    /// Checks if a double value is nearly equal to a nullable double value, using the default tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The nullable value to compare against.</param>
    /// <returns>Returns true if the nullable value has a value and both are nearly equal within the default tolerance.</returns>
    public static bool NearlyEqual(this double value, double? compareTo) =>
        compareTo.HasValue && NearlyEqual(value, compareTo.Value);

    /// <summary>
    /// Checks if a double value is nearly zero, allowing for a specified tolerance.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="epsilon">The acceptable difference from zero.</param>
    /// <returns>Returns true if the value is nearly zero within the specified tolerance.</returns>
    public static bool NearlyZero(this double value, double epsilon) => Math.Abs(value) <= epsilon;

    /// <summary>
    /// Checks if a double value is nearly zero, using the default tolerance.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>Returns true if the value is nearly zero within the default tolerance.</returns>
    public static bool NearlyZero(this double value) => NearlyZero(value, Epsilon);

    /// <summary>
    /// Checks if a nullable double value is nearly zero, using the default tolerance.
    /// </summary>
    /// <param name="value">The nullable value to check.</param>
    /// <returns>Returns true if the value has a value and is nearly zero within the default tolerance.</returns>
    public static bool NearlyZero(this double? value) => value.HasValue && NearlyZero(value.Value);

    /// <summary>
    /// Checks if a nullable double value is nearly zero or null, using the default tolerance.
    /// </summary>
    /// <param name="value">The nullable value to check.</param>
    /// <returns>Returns true if the value is null or is nearly zero within the default tolerance.</returns>
    public static bool NearlyZeroOrNull(this double? value) => !value.HasValue || NearlyZero(value.Value);

    /// <summary>
    /// Checks if a float value is nearly zero, allowing for a specified tolerance.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="epsilon">The acceptable difference from zero.</param>
    /// <returns>Returns true if the value is nearly zero within the specified tolerance.</returns>
    public static bool NearlyZero(this float value, float epsilon) => Math.Abs(value) <= epsilon;

    /// <summary>
    /// Checks if a float value is nearly zero, using the default tolerance.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>Returns true if the value is nearly zero within the default tolerance.</returns>
    public static bool NearlyZero(this float value) => NearlyZero(value, (float)Epsilon);

    /// <summary>
    /// Checks if two float values are nearly equal, using the default tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The value to compare against.</param>
    /// <returns>Returns true if the two values are nearly equal within the default tolerance.</returns>
    public static bool NearlyEqual(this float value, float compareTo) => Math.Abs(value - compareTo) <= Epsilon;

    /// <summary>
    /// Checks if a double value is greater than or nearly equal to another value, allowing for the default tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The value to compare against.</param>
    /// <returns>Returns true if the first value is greater than or nearly equal to the second value.</returns>
    public static bool GreaterThanOrNearlyEqual(this double value, double compareTo) =>
        value > compareTo || NearlyEqual(value, compareTo);

    /// <summary>
    /// Checks if a double value is less than or nearly equal to another value, allowing for the default tolerance.
    /// </summary>
    /// <param name="value">The first value to compare.</param>
    /// <param name="compareTo">The value to compare against.</param>
    /// <returns>Returns true if the first value is less than or nearly equal to the second value.</returns>
    public static bool LessThanOrNearlyEqual(this double value, double compareTo) =>
        value < compareTo || NearlyEqual(value, compareTo);
}
