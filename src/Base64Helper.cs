namespace Helpers;

using System.Text;

/// <summary>
/// Provides methods for working with Base64 encoding and decoding.
/// </summary>
public static class Base64Helper
{
    /// <summary>
    /// Safely decodes a Base64-encoded string into a byte array.
    /// <para>
    /// The method replaces URL-safe characters ('-' and '_') with the standard Base64 characters ('+' and '/').
    /// It then ensures that the string length is a multiple of 4 by appending '=' padding characters as necessary.
    /// </para>
    /// <para>
    /// If the input string is null, empty, or consists only of white spaces, the method returns an empty byte array.
    /// In case of any errors during the decoding process (e.g., if the input is not valid Base64), the method also returns an empty byte array.
    /// </para>
    /// </summary>
    /// <param name="input">The Base64-encoded string to decode.</param>
    /// <returns>A byte array representing the decoded data, or an empty byte array if the input is invalid or an error occurs.</returns>
    public static byte[] SafeConvertFromBase64String(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Array.Empty<byte>();
        }

        try
        {
            // Tente converter diretamente a entrada
            return Convert.FromBase64String(input);
        }
        catch (FormatException)
        {
            return Array.Empty<byte>();
        }
    }



    /// <summary>
    /// Encodes a byte array into a Base64-encoded string.
    /// </summary>
    /// <param name="input">The byte array to encode.</param>
    /// <returns>A Base64-encoded string representing the input byte array, or an empty string if the input is null or empty.</returns>
    public static string EncodeToBase64String(byte[]? input)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        return Convert.ToBase64String(input);
    }

    /// <summary>
    /// Decodes a Base64-encoded string into a byte array with an option to throw an exception if the input is invalid.
    /// </summary>
    /// <param name="input">The Base64-encoded string to decode.</param>
    /// <param name="throwOnInvalidInput">If set to true, throws an exception when the input is invalid.</param>
    /// <returns>A byte array representing the decoded data, or an empty byte array if the input is invalid and <paramref name="throwOnInvalidInput"/> is false.</returns>
    /// <exception cref="FormatException">Thrown if the input is not a valid Base64 string and <paramref name="throwOnInvalidInput"/> is true.</exception>
    public static byte[] ConvertFromBase64String(string? input, bool throwOnInvalidInput)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Array.Empty<byte>();
        }

        try
        {
            StringBuilder working = new StringBuilder(input);
            working.Replace('-', '+').Replace('_', '/');

            while (working.Length % 4 != 0)
            {
                working.Append('=');
            }

            return Convert.FromBase64String(working.ToString());
        }
        catch (FormatException) when (!throwOnInvalidInput)
        {
            return Array.Empty<byte>();
        }
    }
}
