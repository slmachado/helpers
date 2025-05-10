using System.ComponentModel;

namespace Helpers;

public static class EnumHelper
{
    /// <summary>
    /// Gets a list of all values in the enum.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>An IEnumerable of the enum values.</returns>
    public static IEnumerable<T> GetList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    /// <summary>
    /// Parses an integer value to the corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The integer value to parse.</param>
    /// <returns>The corresponding enum value, or the first value if parsing fails.</returns>
    public static T Parse<T>(int value) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        return Enum.GetValues(typeof(T)).Cast<T>().First();
    }

    /// <summary>
    /// Parses a string value to the corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The string value to parse.</param>
    /// <returns>The corresponding enum value, or the first value if parsing fails.</returns>
    public static T Parse<T>(string value) where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out T enumValue) && Enum.IsDefined(typeof(T), enumValue))
        {
            return enumValue;
        }

        return Enum.GetValues(typeof(T)).Cast<T>().First();
    }

    /// <summary>
    /// Attempts to parse an integer value to the corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The integer value to parse.</param>
    /// <param name="enumValue">The corresponding enum value if parsing is successful.</param>
    /// <returns>True if the parsing is successful; otherwise, false.</returns>
    public static bool TryParse<T>(int value, out T enumValue) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            enumValue = (T)Enum.ToObject(typeof(T), value);
            return true;
        }

        enumValue = Enum.GetValues(typeof(T)).Cast<T>().First();
        return false;
    }

    /// <summary>
    /// Attempts to parse a string value to the corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The string value to parse.</param>
    /// <param name="enumValue">The corresponding enum value if parsing is successful.</param>
    /// <returns>True if the parsing is successful; otherwise, false.</returns>
    public static bool TryParse<T>(string value, out T enumValue) where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out T parsedValue) && Enum.IsDefined(typeof(T), parsedValue))
        {
            enumValue = parsedValue;
            return true;
        }

        enumValue = Enum.GetValues(typeof(T)).Cast<T>().First();
        return false;
    }

    /// <summary>
    /// Gets the attribute of a specified type associated with an enum value.
    /// </summary>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <param name="enumVal">The enum value.</param>
    /// <returns>The attribute of the specified type, or null if not found.</returns>
    public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? (T)attributes[0] : null;
    }

    /// <summary>
    /// Gets the description attribute of an enum value.
    /// </summary>
    /// <param name="enumVal">The enum value.</param>
    /// <returns>The description attribute if present, otherwise the enum value as a string.</returns>
    public static string GetDescription(this Enum enumVal)
    {
        var attribute = enumVal.GetAttributeOfType<DescriptionAttribute>();
        return attribute == null ? enumVal.ToString() : attribute.Description;
    }

    /// <summary>
    /// Converts the enum values to a list of their names and integer values.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>A list of KeyValuePairs where the key is the name and the value is the integer value of the enum.</returns>
    public static List<KeyValuePair<string, int>> ToList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new KeyValuePair<string, int>(e.ToString(), Convert.ToInt32(e)))
            .ToList();
    }

    /// <summary>
    /// Checks if an integer value is defined in the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The integer value to check.</param>
    /// <returns>True if the value is defined in the enum, otherwise false.</returns>
    public static bool IsDefined<T>(int value) where T : Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }


    /// <summary>
    /// Checks if a string value is defined in the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The string value to check.</param>
    /// <returns>True if the value is defined in the enum, otherwise false.</returns>
    public static bool IsDefined<T>(string value) where T : Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Normalizar para comparação case-insensitive
        return Enum.GetNames(typeof(T))
                .Any(name => string.Equals(name, value, StringComparison.OrdinalIgnoreCase));
    }


    /// <summary>
    /// Gets the names of the enum values as a list of strings.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>A list of the names of the enum values.</returns>
    public static List<string> GetNames<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T)).ToList();
    }

    /// <summary>
    /// Gets the values of the enum as a list of integers.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>A list of the values of the enum.</returns>
    public static List<int> GetValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<int>().ToList();
    }


    /// <summary>
    /// Gets the default value of the specified enum type.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>The default value of the enum type.</returns>
    public static T GetDefaultValue<T>() where T : struct, Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().First();
    }


    /// <summary>
    /// Converts the enum values to a dictionary of their integer values and descriptions.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <returns>A dictionary where the key is the integer value and the value is the description of the enum.</returns>
    public static Dictionary<int, string> ConvertToDescriptionDictionary<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToDictionary(e => Convert.ToInt32(e), e => e.GetDescription());
    }
}
