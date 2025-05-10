namespace Helpers;

using System.Globalization;
using System.IO;

/// <summary>
/// Helper class for performing common file operations.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Removes illegal characters from a file name or path.
    /// </summary>
    /// <param name="input">The input string containing the file name or path.</param>
    /// <param name="replacement">The string to replace illegal characters with.</param>
    /// <returns>A sanitized file name or path.</returns>
    public static string RemoveIllegalFileNameChars(string input, string replacement = "")
    {
        // Manually list common illegal characters for file names
        char[] illegalChars = { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };

        foreach (char c in illegalChars)
        {
            input = input.Replace(c.ToString(), replacement);
        }

        return input;
    }


    /// <summary>
    /// Reads all lines from a text file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>An array of lines from the file.</returns>
    public static string[] ReadAllLines(string filePath)
    {
        ValidateFilePath(filePath);
        return File.ReadAllLines(filePath);
    }

    /// <summary>
    /// Writes all lines to a text file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="lines">The lines to write to the file.</param>
    public static void WriteAllLines(string filePath, string[] lines)
    {
        ValidateFilePath(filePath);
        File.WriteAllLines(filePath, lines);
    }

    /// <summary>
    /// Reads all text from a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>The text from the file.</returns>
    public static string ReadAllText(string filePath)
    {
        ValidateFilePath(filePath);
        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// Writes text to a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="text">The text to write to the file.</param>
    public static void WriteAllText(string filePath, string text)
    {
        ValidateFilePath(filePath);
        File.WriteAllText(filePath, text);
    }

    /// <summary>
    /// Appends text to a file, creating the file if it does not exist.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="text">The text to append to the file.</param>
    public static void AppendAllText(string filePath, string text)
    {
        ValidateFilePath(filePath);
        File.AppendAllText(filePath, text);
    }

    /// <summary>
    /// Checks if a file exists.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    public static bool FileExists(string filePath)
    {
        ValidateFilePath(filePath);
        return File.Exists(filePath);
    }

    /// <summary>
    /// Deletes a file.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    public static void DeleteFile(string filePath)
    {
        ValidateFilePath(filePath);
        if (FileExists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Copies a file to a new location.
    /// </summary>
    /// <param name="sourceFilePath">The source file path.</param>
    /// <param name="destFilePath">The destination file path.</param>
    /// <param name="overwrite">True to overwrite the destination file if it exists; otherwise, false.</param>
    public static void CopyFile(string sourceFilePath, string destFilePath, bool overwrite = false)
    {
        ValidateFilePath(sourceFilePath);
        ValidateFilePath(destFilePath);
        File.Copy(sourceFilePath, destFilePath, overwrite);
    }

    /// <summary>
    /// Moves a file to a new location.
    /// </summary>
    /// <param name="sourceFilePath">The source file path.</param>
    /// <param name="destFilePath">The destination file path.</param>
    public static void MoveFile(string sourceFilePath, string destFilePath)
    {
        ValidateFilePath(sourceFilePath);
        ValidateFilePath(destFilePath);
        File.Move(sourceFilePath, destFilePath);
    }

    /// <summary>
    /// Validates if the file path is not null or empty.
    /// </summary>
    /// <param name="filePath">The file path to validate.</param>
    private static void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }
    }
}

/// <summary>
/// Extension methods for working with file sizes.
/// </summary>
public static class FileExtensionMethods
{
    /// <summary>
    /// Converts a long value representing bytes into a formatted file size string.
    /// </summary>
    /// <param name="l">The size in bytes.</param>
    /// <returns>A formatted file size string (e.g., "1.23 MB").</returns>
    public static string ToFileSize(this long l)
    {
        return string.Format(new FileSizeFormatProvider(), "{0:fs}", l);
    }
}

/// <summary>
/// Custom format provider for formatting file sizes.
/// </summary>
public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
{
    private const string FileSizeFormat = "fs";
    private const decimal OneKiloByte = 1024M;
    private const decimal OneMegaByte = OneKiloByte * 1024M;
    private const decimal OneGigaByte = OneMegaByte * 1024M;

    public object? GetFormat(Type? formatType)
    {
        return formatType == typeof(ICustomFormatter) ? this : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (arg == null)
        {
            return string.Empty;
        }

        var nonNullFormatProvider = formatProvider ?? CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(format) || !format.StartsWith(FileSizeFormat))
        {
            return DefaultFormat(format, arg, nonNullFormatProvider) ?? string.Empty;
        }

        if (arg is string)
        {
            return DefaultFormat(format, arg, nonNullFormatProvider) ?? string.Empty;
        }

        decimal size;
        try
        {
            size = Convert.ToDecimal(arg);
        }
        catch (InvalidCastException)
        {
            return DefaultFormat(format, arg, nonNullFormatProvider) ?? string.Empty;
        }

        string suffix;
        switch (size)
        {
            case > OneGigaByte:
                size /= OneGigaByte;
                suffix = "GB";
                break;
            case > OneMegaByte:
                size /= OneMegaByte;
                suffix = "MB";
                break;
            case > OneKiloByte:
                size /= OneKiloByte;
                suffix = "kB";
                break;
            default:
                suffix = "B";
                break;
        }

        var precision = format.Length > 2 ? format[2..] : "2";

        return string.Format("{{0:N" + precision + "}} {1}", size, suffix);
    }

    private static string? DefaultFormat(string? format, object arg, IFormatProvider formatProvider)
    {
        return arg is IFormattable formattableArg ? formattableArg.ToString(format, formatProvider) : arg.ToString();
    }
}
