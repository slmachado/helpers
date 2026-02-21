namespace Helpers;

/// <summary>
/// Provides utility methods for working with GUIDs.
/// </summary>
public static class GuidHelper
{
    /// <summary>
    /// Checks whether the given string is a valid GUID.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if the string is a valid GUID; otherwise, false.</returns>
    public static bool IsValid(string? value)
        => !string.IsNullOrWhiteSpace(value) && Guid.TryParse(value, out _);

    /// <summary>
    /// Converts a GUID to a URL-safe Base64 short representation (22 characters).
    /// </summary>
    /// <param name="guid">The GUID to convert.</param>
    /// <returns>A 22-character URL-safe Base64 string.</returns>
    public static string ToShortGuid(Guid guid)
    {
        return Convert.ToBase64String(guid.ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .TrimEnd('=');
    }

    /// <summary>
    /// Converts a short GUID string back to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="shortGuid">The 22-character short GUID string.</param>
    /// <returns>The original <see cref="Guid"/>.</returns>
    /// <exception cref="FormatException">Thrown when the input is not a valid short GUID.</exception>
    public static Guid FromShortGuid(string shortGuid)
    {
        if (string.IsNullOrWhiteSpace(shortGuid))
            throw new ArgumentNullException(nameof(shortGuid));

        var base64 = shortGuid
            .Replace("_", "/")
            .Replace("-", "+")
            + "==";

        try
        {
            var bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }
        catch (Exception ex)
        {
            throw new FormatException($"'{shortGuid}' is not a valid short GUID.", ex);
        }
    }

    /// <summary>
    /// Returns <see cref="Guid.Empty"/> if the input is null or whitespace; otherwise parses the GUID.
    /// </summary>
    /// <param name="value">The GUID string.</param>
    /// <returns>The parsed GUID or <see cref="Guid.Empty"/>.</returns>
    public static Guid ParseOrEmpty(string? value)
        => Guid.TryParse(value, out var result) ? result : Guid.Empty;

    /// <summary>
    /// Tries to parse the string to a GUID. Returns null if invalid.
    /// </summary>
    /// <param name="value">The GUID string.</param>
    /// <returns>The parsed <see cref="Guid"/>, or null if invalid.</returns>
    public static Guid? TryParse(string? value)
        => Guid.TryParse(value, out var result) ? result : null;

    /// <summary>
    /// Generates a sequential GUID using the current UTC timestamp combined with
    /// random bytes, producing values that sort well in databases (e.g. SQL Server).
    /// </summary>
    /// <returns>A new sequential <see cref="Guid"/>.</returns>
    public static Guid GenerateSequential()
    {
        var randomBytes = Guid.NewGuid().ToByteArray();
        var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

        // Place timestamp in the last 8 bytes for sequential ordering
        randomBytes[8]  = timestamp[1];
        randomBytes[9]  = timestamp[0];
        randomBytes[10] = timestamp[7];
        randomBytes[11] = timestamp[6];
        randomBytes[12] = timestamp[5];
        randomBytes[13] = timestamp[4];
        randomBytes[14] = timestamp[3];
        randomBytes[15] = timestamp[2];

        return new Guid(randomBytes);
    }
}
