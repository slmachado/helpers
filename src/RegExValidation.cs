using System.Globalization;
using System.Text.RegularExpressions;

namespace Helpers;

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

    public static bool CheckPassword(string password) => PasswordRegex.IsMatch(password);

    public static bool IsValidEmail(string email) => EmailRegex.IsMatch(email);

    public static bool IsValidPhoneNumber(string phoneNumber) => PhoneNumberRegex.IsMatch(phoneNumber);

    public static bool IsValidURL(string url) => UrlRegex.IsMatch(url);

    public static bool IsValidPostalCode(string postalCode) => PostalCodeRegex.IsMatch(postalCode);

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

    public static bool IsValidIPv4(string ipAddress) => IPv4Regex.IsMatch(ipAddress);

    public static bool IsValidIPv6(string ipAddress) => IPv6Regex.IsMatch(ipAddress);

    public static bool IsValidSSN(string ssn) => SSNRegex.IsMatch(ssn);

    public static bool IsValidUsername(string username) => UsernameRegex.IsMatch(username);

    public static bool IsValidDate(string date, params string[] dateFormats)
    {
        return dateFormats.Any(format => DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
    }
}
