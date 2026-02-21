using System.Text.Json;
using System.Text.Json.Nodes;

namespace Helpers;

/// <summary>
/// Provides utility methods for JSON manipulation that complement <see cref="SerializationHelper"/>.
/// </summary>
public static class JsonHelper
{
    private static readonly JsonSerializerOptions PrettyOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions CamelCaseOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Attempts to deserialize a JSON string to the specified type.
    /// Returns the default value if deserialization fails.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="json">The JSON string.</param>
    /// <param name="result">The deserialized object, or default on failure.</param>
    /// <returns>True if deserialization succeeded; otherwise, false.</returns>
    public static bool TryDeserialize<T>(string? json, out T? result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(json)) return false;

        try
        {
            result = JsonSerializer.Deserialize<T>(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Formats a JSON string with indentation (pretty-print).
    /// </summary>
    /// <param name="json">The compact JSON string.</param>
    /// <returns>A pretty-printed JSON string.</returns>
    public static string Prettify(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentNullException(nameof(json));

        using var doc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(doc.RootElement, PrettyOptions);
    }

    /// <summary>
    /// Minifies a JSON string by removing unnecessary whitespace.
    /// </summary>
    /// <param name="json">The JSON string to minify.</param>
    /// <returns>A compact JSON string.</returns>
    public static string Minify(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentNullException(nameof(json));

        using var doc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(doc.RootElement);
    }

    /// <summary>
    /// Merges two JSON objects. Properties from <paramref name="patch"/> override those in <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The base JSON object string.</param>
    /// <param name="patch">The JSON object string whose properties take precedence.</param>
    /// <returns>A merged JSON string.</returns>
    public static string Merge(string source, string patch)
    {
        if (string.IsNullOrWhiteSpace(source)) throw new ArgumentNullException(nameof(source));
        if (string.IsNullOrWhiteSpace(patch))  throw new ArgumentNullException(nameof(patch));

        var sourceNode = JsonNode.Parse(source) as JsonObject
            ?? throw new ArgumentException("source must be a JSON object.", nameof(source));
        var patchNode  = JsonNode.Parse(patch)  as JsonObject
            ?? throw new ArgumentException("patch must be a JSON object.",  nameof(patch));

        foreach (var (key, value) in patchNode)
            sourceNode[key] = value?.DeepClone();

        return sourceNode.ToJsonString();
    }

    /// <summary>
    /// Serializes an object to a camelCase JSON string.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A camelCase JSON string.</returns>
    public static string SerializeCamelCase<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        return JsonSerializer.Serialize(obj, CamelCaseOptions);
    }

    /// <summary>
    /// Tries to read a top-level property value from a JSON string by key.
    /// </summary>
    /// <param name="json">The JSON object string.</param>
    /// <param name="key">The property key.</param>
    /// <param name="value">The raw JSON value string if found.</param>
    /// <returns>True if the key was found; otherwise, false.</returns>
    public static bool TryGetProperty(string json, string key, out string? value)
    {
        value = null;
        if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(key)) return false;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty(key, out var element))
            {
                value = element.ToString();
                return true;
            }
        }
        catch (JsonException) { }

        return false;
    }

    /// <summary>
    /// Flattens a nested JSON object into a dictionary with dot-separated keys.
    /// Arrays are not flattened.
    /// </summary>
    /// <param name="json">The JSON object string to flatten.</param>
    /// <returns>A dictionary of dot-separated keys and their string values.</returns>
    public static Dictionary<string, string?> Flatten(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentNullException(nameof(json));

        var result = new Dictionary<string, string?>();
        using var doc = JsonDocument.Parse(json);
        FlattenElement(doc.RootElement, string.Empty, result);
        return result;
    }

    private static void FlattenElement(JsonElement element, string prefix, Dictionary<string, string?> result)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                    FlattenElement(prop.Value, key, result);
                }
                break;
            default:
                result[prefix] = element.ToString();
                break;
        }
    }
}
