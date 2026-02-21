using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Helpers;

/// <summary>
/// Provides methods for generating and validating passwords.
/// </summary>
public static class PasswordHelper
{
    private const string LowerChars   = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperChars   = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string DigitChars   = "0123456789";
    private const string SymbolChars  = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    /// <summary>
    /// Generates a random password with the specified options.
    /// </summary>
    /// <param name="length">The length of the password. Must be at least 4.</param>
    /// <param name="useUppercase">Include uppercase letters.</param>
    /// <param name="useDigits">Include digits.</param>
    /// <param name="useSymbols">Include special symbols.</param>
    /// <returns>A randomly generated password string.</returns>
    public static string Generate(int length = 12, bool useUppercase = true, bool useDigits = true, bool useSymbols = true)
    {
        if (length < 4)
            throw new ArgumentOutOfRangeException(nameof(length), "Password length must be at least 4.");

        var pool = new StringBuilder(LowerChars);
        var required = new List<char>();

        required.Add(LowerChars[RandomInt(LowerChars.Length)]);

        if (useUppercase)
        {
            pool.Append(UpperChars);
            required.Add(UpperChars[RandomInt(UpperChars.Length)]);
        }
        if (useDigits)
        {
            pool.Append(DigitChars);
            required.Add(DigitChars[RandomInt(DigitChars.Length)]);
        }
        if (useSymbols)
        {
            pool.Append(SymbolChars);
            required.Add(SymbolChars[RandomInt(SymbolChars.Length)]);
        }

        var poolStr = pool.ToString();
        var chars = new char[length];

        for (int i = 0; i < length; i++)
            chars[i] = poolStr[RandomInt(poolStr.Length)];

        // Guarantee at least one of each required character type
        for (int i = 0; i < required.Count; i++)
            chars[i] = required[i];

        // Fisher-Yates shuffle to distribute required chars randomly
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomInt(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }

    /// <summary>
    /// Checks if a password is considered strong.
    /// A strong password must have at least 8 characters, one uppercase, one digit and one symbol.
    /// </summary>
    /// <param name="password">The password to evaluate.</param>
    /// <returns>True if the password is strong; otherwise, false.</returns>
    public static bool IsStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        return Regex.IsMatch(password, @"[A-Z]")   // uppercase
            && Regex.IsMatch(password, @"[a-z]")   // lowercase
            && Regex.IsMatch(password, @"[0-9]")   // digit
            && Regex.IsMatch(password, @"[^a-zA-Z0-9]"); // symbol
    }

    /// <summary>
    /// Evaluates the strength of a password on a scale from 0 to 4.
    /// </summary>
    /// <param name="password">The password to evaluate.</param>
    /// <returns>
    /// 0 = Very weak, 1 = Weak, 2 = Medium, 3 = Strong, 4 = Very strong.
    /// </returns>
    public static int GetStrengthScore(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return 0;

        int score = 0;
        if (password.Length >= 8)  score++;
        if (password.Length >= 12) score++;
        if (Regex.IsMatch(password, @"[A-Z]") && Regex.IsMatch(password, @"[a-z]")) score++;
        if (Regex.IsMatch(password, @"[0-9]"))     score++;
        if (Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score++;

        return Math.Min(score, 4);
    }

    /// <summary>
    /// Hashes a password using SHA256.
    /// For production use, prefer a proper key-derivation function such as BCrypt or PBKDF2.
    /// </summary>
    /// <param name="password">The plain-text password.</param>
    /// <returns>A hex-encoded SHA256 hash of the password.</returns>
    public static string HashSha256(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }

    /// <summary>
    /// Verifies a plain-text password against a SHA256 hash.
    /// </summary>
    /// <param name="password">The plain-text password.</param>
    /// <param name="hash">The previously computed hash.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public static bool VerifySha256(string password, string hash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            return false;

        return string.Equals(HashSha256(password), hash, StringComparison.OrdinalIgnoreCase);
    }

    private static int RandomInt(int maxExclusive)
        => RandomNumberGenerator.GetInt32(maxExclusive);
}
