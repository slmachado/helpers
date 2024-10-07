using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using System.IO.Compression;

namespace Helpers;

public static class SerializationHelper
{
    /// <summary>
    /// Serializes an object to an XmlDocument.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>An XmlDocument containing the serialized object.</returns>
    public static XmlDocument SerializeToXml<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var xmlDoc = new XmlDocument();
        using var xmlWriter = xmlDoc.CreateNavigator()?.AppendChild();

        if (xmlWriter == null)
        {
            throw new InvalidOperationException("Failed to create an XML writer.");
        }

        var xmlSerializer = new XmlSerializer(typeof(T));
        xmlSerializer.Serialize(xmlWriter, obj);

        return xmlDoc;
    }


    /// <summary>
    /// Deserializes an object from an XmlReader.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="xml">The XmlReader containing the serialized object.</param>
    /// <returns>The deserialized object.</returns>
    public static T DeserializeFromXml<T>(XmlReader xml)
    {
        if (xml == null) throw new ArgumentNullException(nameof(xml));

        var xmlSerializer = new XmlSerializer(typeof(T));
        return (T)xmlSerializer.Deserialize(xml)!;
    }

    /// <summary>
    /// Serializes an object to a string using DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A string containing the serialized object.</returns>
    public static string SerializeDataContract<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        using var stream = new MemoryStream();
        var serializer = new DataContractSerializer(typeof(T));
        serializer.WriteObject(stream, obj);
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    /// <summary>
    /// Deserializes an object from a string using DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="data">The string containing the serialized object.</param>
    /// <returns>The deserialized object.</returns>
    public static T DeserializeDataContract<T>(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var serializer = new DataContractSerializer(typeof(T));
        return (T)serializer.ReadObject(stream)!;
    }

    /// <summary>
    /// Serializes an object to a JSON string using System.Text.Json.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A JSON string containing the serialized object.</returns>
    public static string SerializeToJson<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        return JsonSerializer.Serialize(obj);
    }

    /// <summary>
    /// Deserializes an object from a JSON string using System.Text.Json.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="json">The JSON string containing the serialized object.</param>
    /// <returns>The deserialized object.</returns>
    public static T DeserializeFromJson<T>(string json)
    {
        if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));

        return JsonSerializer.Deserialize<T>(json)!;
    }

    /// <summary>
    /// Validates if a string is a well-formed XML.
    /// </summary>
    /// <param name="xml">The XML string to validate.</param>
    /// <returns>True if the string is a well-formed XML; otherwise, false.</returns>
    public static bool IsValidXml(string xml)
    {
        if (string.IsNullOrEmpty(xml)) return false;

        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }

    /// <summary>
    /// Validates if a string is a well-formed JSON.
    /// </summary>
    /// <param name="json">The JSON string to validate.</param>
    /// <returns>True if the string is a well-formed JSON; otherwise, false.</returns>
    public static bool IsValidJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return false;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Formats an XmlDocument to a pretty-printed string.
    /// </summary>
    /// <param name="xmlDoc">The XmlDocument to format.</param>
    /// <returns>A pretty-printed string representation of the XmlDocument.</returns>
    public static string FormatXml(XmlDocument xmlDoc)
    {
        if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc));

        using var stringWriter = new StringWriter();
        using var xmlTextWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
        xmlDoc.WriteTo(xmlTextWriter);
        xmlTextWriter.Flush();
        return stringWriter.GetStringBuilder().ToString();
    }

    /// <summary>
    /// Compresses an object to a byte array using GZip.
    /// </summary>
    /// <typeparam name="T">The type of the object to compress.</typeparam>
    /// <param name="obj">The object to compress.</param>
    /// <returns>A byte array containing the compressed object.</returns>
    public static byte[] Compress<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var json = SerializeToJson(obj);
        if (string.IsNullOrEmpty(json))
        {
            throw new InvalidOperationException("Serialization produced an empty JSON string.");
        }

        var bytes = Encoding.UTF8.GetBytes(json);
        using var memoryStream = new MemoryStream();
        using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
        gzipStream.Write(bytes, 0, bytes.Length);
        gzipStream.Flush();
        return memoryStream.ToArray();
    }


    /// <summary>
    /// Decompresses an object from a byte array using GZip.
    /// </summary>
    /// <typeparam name="T">The type of the object to decompress.</typeparam>
    /// <param name="compressedData">The byte array containing the compressed object.</param>
    /// <returns>The decompressed object.</returns>
    public static T Decompress<T>(byte[] compressedData)
    {
        if (compressedData == null || compressedData.Length == 0)
            throw new ArgumentNullException(nameof(compressedData));

        using var compressedStream = new MemoryStream(compressedData);
        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var resultStream = new MemoryStream();
        
        try
        {
            gzipStream.CopyTo(resultStream);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to decompress the data.", ex);
        }

        var json = Encoding.UTF8.GetString(resultStream.ToArray());

        if (string.IsNullOrEmpty(json))
            throw new InvalidOperationException("Decompressed data is null or empty.");

        return DeserializeFromJson<T>(json);
    }



    /// <summary>
    /// Serializes an object to a byte array using BinaryFormatter.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A byte array containing the serialized object.</returns>
    [Obsolete("BinaryFormatter is obsolete and not secure. Use SerializeToJsonByteArray<T> instead.")]
    public static byte[] SerializeToBinary<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        using var memoryStream = new MemoryStream();
        var formatter = new BinaryFormatter();
        formatter.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Serializes an object to a byte array using JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A byte array containing the serialized object.</returns>
    public static byte[] SerializeToJsonByteArray<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        // Serializa o objeto para uma string JSON e, em seguida, converte em bytes UTF-8
        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }


    /// <summary>
    /// Deserializes an object from a byte array using BinaryFormatter.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="data">The byte array containing the serialized object.</param>
    /// <returns>The deserialized object.</returns>
    [Obsolete("BinaryFormatter is obsolete and not secure. Use DeserializeFromJsonByteArray<T> instead.")]
    public static T DeserializeFromBinary<T>(byte[] data)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException(nameof(data));

        using var memoryStream = new MemoryStream(data);
        var formatter = new BinaryFormatter();
        return (T)formatter.Deserialize(memoryStream)!;
    }


    /// <summary>
    /// Deserializes an object of type <typeparamref name="T"/> from a JSON byte array.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="data">The byte array containing the JSON representation of the object.</param>
    /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided byte array is null or empty.</exception>
    public static T DeserializeFromJsonByteArray<T>(byte[] data)
    {
        if (data == null || data.Length == 0) throw new ArgumentNullException(nameof(data));

        var json = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
