using System.Globalization;
using System.Text.RegularExpressions;
// ReSharper disable InconsistentNaming

namespace Helpers;

/// <summary>
/// Provides regular expression-based validation methods for various formats including password, email, phone number, URLs, postal codes, credit cards, IP addresses, SSNs, usernames, and dates.
/// </summary>
public static class RegExValidation
{
    private static readonly Regex PasswordRegex = new(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,30}", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex PhoneNumberRegex = new(@"^\+?[1-9]\d{9,14}$", RegexOptions.Compiled);
    private static readonly Regex UrlRegex = new(@"^(https?):\/\/[^\s/$.?#].[^\s]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex PostalCodeRegex = new(@"^\d{4,5}(-\d{4})?$", RegexOptions.Compiled);
    private static readonly Regex CreditCardRegex = new(@"^\d{16}$", RegexOptions.Compiled);
    private static readonly Regex IPv4Regex = new(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled);
    private static readonly Regex IPv6Regex = new(@"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9])?[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9])?[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9])?[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9])?[0-9]))$", RegexOptions.Compiled);
    private static readonly Regex SSNRegex = new(@"^\d{3}-\d{2}-\d{4}$", RegexOptions.Compiled);
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9]{3,16}$", RegexOptions.Compiled);

    /// <summary>
    /// Checks if the given password meets the required complexity.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password is valid, otherwise false.</returns>
    public static bool CheckPassword(string password) => PasswordRegex.IsMatch(password);

    /// <summary>
    /// Validates if the given string is a valid email address.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email is valid, otherwise false.</returns>
    public static bool IsValidEmail(string email) => EmailRegex.IsMatch(email);

    /// <summary>
    /// Validates if the given string is a valid phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>True if the phone number is valid, otherwise false.</returns>
    public static bool IsValidPhoneNumber(string phoneNumber) => PhoneNumberRegex.IsMatch(phoneNumber);

    /// <summary>
    /// Validates if the given string is a valid URL.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns>True if the URL is valid, otherwise false.</returns>
    public static bool IsValidURL(string url) => UrlRegex.IsMatch(url);

    /// <summary>
    /// Validates if the given string is a valid postal code.
    /// </summary>
    /// <param name="postalCode">The postal code to validate.</param>
    /// <returns>True if the postal code is valid, otherwise false.</returns>
    public static bool IsValidPostalCode(string postalCode) => PostalCodeRegex.IsMatch(postalCode);

    /// <summary>
    /// Validates if the given string is a valid credit card number using the Luhn algorithm.
    /// </summary>
    /// <param name="creditCardNumber">The credit card number to validate.</param>
    /// <returns>True if the credit card number is valid, otherwise false.</returns>
    public static bool IsValidCreditCard(string creditCardNumber)
    {
        if (!CreditCardRegex.IsMatch(creditCardNumber))
            return false;

        int sum = 0;
        bool alternate = false;
        for (int i = creditCardNumber.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(creditCardNumber[i].ToString(), CultureInfo.InvariantCulture);
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                {
                    n -= 9;
                }
            }
            sum += n;
            alternate = !alternate;
        }
        return (sum % 10 == 0);
    }

    /// <summary>
    /// Validates if the given string is a valid IPv4 address.
    /// </summary>
    /// <param name="ipAddress">The IPv4 address to validate.</param>
    /// <returns>True if the IPv4 address is valid, otherwise false.</returns>
    public static bool IsValidIPv4(string ipAddress) => IPv4Regex.IsMatch(ipAddress);

    /// <summary>
    /// Validates if the given string is a valid IPv6 address.
    /// </summary>
    /// <param name="ipAddress">The IPv6 address to validate.</param>
    /// <returns>True if the IPv6 address is valid, otherwise false.</returns>
    public static bool IsValidIPv6(string ipAddress) => IPv6Regex.IsMatch(ipAddress);

    /// <summary>
    /// Validates if the given string is a valid Social Security Number (SSN).
    /// </summary>
    /// <param name="ssn">The SSN to validate.</param>
    /// <returns>True if the SSN is valid, otherwise false.</returns>
    public static bool IsValidSSN(string ssn) => SSNRegex.IsMatch(ssn);

    /// <summary>
    /// Validates if the given string is a valid username.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <returns>True if the username is valid, otherwise false.</returns>
    public static bool IsValidUsername(string username) => UsernameRegex.IsMatch(username);

    /// <summary>
    /// Validates if the given date string matches any of the specified date formats.
    /// </summary>
    /// <param name="date">The date string to validate.</param>
    /// <param name="dateFormats">An array of date formats to check against.</param>
    /// <returns>True if the date string matches any format, otherwise false.</returns>
    public static bool IsValidDate(string date, params string[] dateFormats)
    {
        return dateFormats.Any(format => DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
    }
}
