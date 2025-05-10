namespace Helpers;

/// <summary>
/// Provides utility methods for working with binary representations of numeric values.
/// </summary>
public static class BinaryHelper
{
    #region Convert to binary string

    /// <summary>
    /// Converts a long value to a 64-bit binary string.
    /// </summary>
    /// <param name="value">The long value to convert to a binary string.</param>
    /// <returns>A 64-bit binary string representation of the specified long value.</returns>
    public static string ToBinary(this long value)
    {
        return Convert.ToString(value, 2).PadLeft(64, '0');
    }

    /// <summary>
    /// Converts an integer value to a 32-bit binary string.
    /// </summary>
    /// <param name="value">The integer value to convert to a binary string.</param>
    /// <returns>A 32-bit binary string representation of the specified integer value.</returns>
    public static string ToBinary(this int value)
    {
        return Convert.ToString(value, 2).PadLeft(32, '0');
    }

    /// <summary>
    /// Converts a short value to a 16-bit binary string.
    /// </summary>
    /// <param name="value">The short value to convert to a binary string.</param>
    /// <returns>A 16-bit binary string representation of the specified short value.</returns>
    public static string ToBinary(this short value)
    {
        return Convert.ToString(value, 2).PadLeft(16, '0');
    }

    #endregion

    #region Get Value from numeric word

    /// <summary>
    /// Gets the boolean value from a specific bit position in a long value using bitwise operations.
    /// </summary>
    /// <param name="value">The long value to evaluate.</param>
    /// <param name="position">The position of the bit to evaluate (0-based from right).</param>
    /// <returns>True if the bit at the specified position is 1, otherwise false.</returns>
    public static bool GetBitValue(this long value, int position)
    {
        if (position is < 0 or >= 64)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 63 for a 64-bit value.");
        }

        return (value & (1L << position)) != 0;
    }

    /// <summary>
    /// Gets the boolean value from a specific bit position in an integer value using bitwise operations.
    /// </summary>
    /// <param name="value">The integer value to evaluate.</param>
    /// <param name="position">The position of the bit to evaluate (0-based from right).</param>
    /// <returns>True if the bit at the specified position is 1, otherwise false.</returns>
    public static bool GetBitValue(this int value, int position)
    {
        if (position is < 0 or >= 32)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 31 for a 32-bit value.");
        }

        return (value & (1 << position)) != 0;
    }

    /// <summary>
    /// Gets the boolean value from a specific bit position in a short value using bitwise operations.
    /// </summary>
    /// <param name="value">The short value to evaluate.</param>
    /// <param name="position">The position of the bit to evaluate (0-based from right).</param>
    /// <returns>True if the bit at the specified position is 1, otherwise false.</returns>
    public static bool GetBitValue(this short value, int position)
    {
        if (position is < 0 or >= 16)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 15 for a 16-bit value.");
        }

        return (value & (1 << position)) != 0;
    }

    /// <summary>
    /// Gets the integer value from a specific bit range in a long value.
    /// </summary>
    /// <param name="value">The long value to evaluate.</param>
    /// <param name="startBitPosition">The start position of the bit range (0-based from right).</param>
    /// <param name="endBitPosition">The end position of the bit range (0-based from right).</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>An integer value from the specified range of bits.</returns>
    public static int GetBitsValue(this long value, int startBitPosition, int endBitPosition)
    {
        if (startBitPosition < 0 || endBitPosition > 64 || startBitPosition >= endBitPosition)
        {
            throw new ArgumentOutOfRangeException($"Start and end positions must be within the range of the 64-bit value, and start must be less than end.");
        }

        var length = endBitPosition - startBitPosition;
        var mask = (1L << length) - 1;
        return (int)((value >> startBitPosition) & mask);
    }

    #endregion

    #region Set and Toggle Bit Value

    /// <summary>
    /// Sets a specific bit in a long value to 1 or 0.
    /// </summary>
    /// <param name="value">The long value to modify.</param>
    /// <param name="position">The position of the bit to set (0-based from right).</param>
    /// <param name="bitValue">The value to set the bit to (true for 1, false for 0).</param>
    /// <returns>A long value with the specified bit set to the given value.</returns>
    public static long SetBitValue(this long value, int position, bool bitValue)
    {
        if (position < 0 || position >= 64)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 63 for a 64-bit value.");
        }

        if (bitValue)
        {
            return value | (1L << position);
        }
        else
        {
            return value & ~(1L << position);
        }
    }

    /// <summary>
    /// Toggles a specific bit in a long value.
    /// </summary>
    /// <param name="value">The long value to modify.</param>
    /// <param name="position">The position of the bit to toggle (0-based from right).</param>
    /// <returns>A long value with the specified bit toggled.</returns>
    public static long ToggleBitValue(this long value, int position)
    {
        if (position < 0 || position >= 64)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be between 0 and 63 for a 64-bit value.");
        }

        return value ^ (1L << position);
    }

    #endregion
}