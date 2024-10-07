namespace Helpers.Tests;

using System;
using System.IO;
using System.Xml;
using Xunit;
using FluentAssertions;
using Helpers;
using System.Runtime.Serialization;

public class SerializationHelperTests
{
    [Fact]
    public void SerializeToXml_ShouldReturnXmlDocument_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var xmlDoc = SerializationHelper.SerializeToXml(obj);

        // Assert
        xmlDoc.Should().NotBeNull();
        xmlDoc.InnerXml.Should().Contain("Test");
    }

    [Fact]
    public void DeserializeFromXml_ShouldReturnObject_WhenXmlIsValid()
    {
        // Arrange
        var xmlString = "<TestClass><Id>1</Id><Name>Test</Name></TestClass>";
        using var reader = XmlReader.Create(new StringReader(xmlString));

        // Act
        var obj = SerializationHelper.DeserializeFromXml<TestClass>(reader);

        // Assert
        obj.Should().NotBeNull();
        obj.Id.Should().Be(1);
        obj.Name.Should().Be("Test");
    }

    [Fact]
    public void SerializeDataContract_ShouldReturnString_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var serialized = SerializationHelper.SerializeDataContract(obj);

        // Assert
        serialized.Should().NotBeNullOrEmpty();
        serialized.Should().Contain("Test");
    }


    [Fact]
    public void DeserializeDataContract_ShouldReturnObject_WhenStringIsValid()
    {
        // Ajuste o XML para incluir o namespace correto
        var serializedString = "<TestClass xmlns=\"http://schemas.datacontract.org/2004/07/Helpers.Tests\"><Id>1</Id><Name>Test</Name></TestClass>";

        var obj = SerializationHelper.DeserializeDataContract<TestClass>(serializedString);

        obj.Should().NotBeNull();
        obj.Id.Should().Be(1);
        obj.Name.Should().Be("Test");
    }


    [Fact]
    public void SerializeToJson_ShouldReturnJsonString_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var json = SerializationHelper.SerializeToJson(obj);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"Name\":\"Test\"");
    }

    [Fact]
    public void DeserializeFromJson_ShouldReturnObject_WhenJsonIsValid()
    {
        // Arrange
        var jsonString = "{\"Id\":1,\"Name\":\"Test\"}";

        // Act
        var obj = SerializationHelper.DeserializeFromJson<TestClass>(jsonString);

        // Assert
        obj.Should().NotBeNull();
        obj.Id.Should().Be(1);
        obj.Name.Should().Be("Test");
    }

    [Fact]
    public void IsValidXml_ShouldReturnTrue_WhenXmlIsWellFormed()
    {
        // Arrange
        var xml = "<TestClass><Id>1</Id><Name>Test</Name></TestClass>";

        // Act
        var result = SerializationHelper.IsValidXml(xml);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidXml_ShouldReturnFalse_WhenXmlIsNotWellFormed()
    {
        // Arrange
        var invalidXml = "<TestClass><Id>1<Name>Test</Name>";

        // Act
        var result = SerializationHelper.IsValidXml(invalidXml);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidJson_ShouldReturnTrue_WhenJsonIsWellFormed()
    {
        // Arrange
        var json = "{\"Id\":1,\"Name\":\"Test\"}";

        // Act
        var result = SerializationHelper.IsValidJson(json);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidJson_ShouldReturnFalse_WhenJsonIsNotWellFormed()
    {
        // Arrange
        var invalidJson = "{\"Id\":1,Name:\"Test\"";

        // Act
        var result = SerializationHelper.IsValidJson(invalidJson);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Compress_ShouldReturnNonEmptyByteArray_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var compressedData = SerializationHelper.Compress(obj);

        // Assert
        compressedData.Should().NotBeNull();
        compressedData.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Compress_ShouldReturnCompressedByteArray_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var compressedData = SerializationHelper.Compress(obj);

        // Assert
        compressedData.Should().NotBeNull();
        compressedData.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Decompress_ShouldReturnOriginalObject_WhenDataIsValid()
    {
        // Arrange
        var originalObj = new TestClass { Id = 1, Name = "Test" };
        var compressedData = SerializationHelper.Compress(originalObj);

        // Act
        var decompressedObj = SerializationHelper.Decompress<TestClass>(compressedData);

        // Assert
        decompressedObj.Should().NotBeNull();
        decompressedObj.Id.Should().Be(originalObj.Id);
        decompressedObj.Name.Should().Be(originalObj.Name);
    }

    [Fact]
    public void SerializeToJsonByteArray_ShouldReturnByteArray_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };

        // Act
        var byteArray = SerializationHelper.SerializeToJsonByteArray(obj);

        // Assert
        byteArray.Should().NotBeNull();
        byteArray.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public void DeserializeFromJsonByteArray_ShouldReturnObject_WhenDataIsValid()
    {
        // Arrange
        var obj = new TestClass { Id = 1, Name = "Test" };
        var byteArray = SerializationHelper.SerializeToJsonByteArray(obj);

        // Act
        var deserializedObj = SerializationHelper.DeserializeFromJsonByteArray<TestClass>(byteArray);

        // Assert
        deserializedObj.Should().NotBeNull();
        deserializedObj.Id.Should().Be(1);
        deserializedObj.Name.Should().Be("Test");
    }


    #region Helper Class

    [DataContract(Name = "TestClass", Namespace = "http://schemas.datacontract.org/2004/07/Helpers.Tests")]
    public class TestClass
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; } = string.Empty;
    }

    #endregion
}

