using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class JsonHelperTests
{
    private record Person(string Name, int Age);

    [Fact]
    public void TryDeserialize_ValidJson_ReturnsTrueAndObject()
    {
        var json = """{"Name":"Alice","Age":30}""";
        var success = JsonHelper.TryDeserialize<Person>(json, out var result);

        success.Should().BeTrue();
        result.Should().BeEquivalentTo(new Person("Alice", 30));
    }

    [Fact]
    public void TryDeserialize_InvalidJson_ReturnsFalseAndDefault()
    {
        var success = JsonHelper.TryDeserialize<Person>("not json", out var result);

        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_NullOrEmpty_ReturnsFalse()
    {
        JsonHelper.TryDeserialize<Person>(null, out _).Should().BeFalse();
        JsonHelper.TryDeserialize<Person>("", out _).Should().BeFalse();
    }

    [Fact]
    public void Prettify_CompactJson_ReturnsIndented()
    {
        var compact = """{"name":"Bob","age":25}""";
        var pretty = JsonHelper.Prettify(compact);

        pretty.Should().Contain("\n");
        pretty.Should().Contain("  ");
    }

    [Fact]
    public void Minify_IndentedJson_ReturnsCompact()
    {
        var indented = """
            {
              "name": "Bob",
              "age": 25
            }
            """;
        var minified = JsonHelper.Minify(indented);

        minified.Should().NotContain("\n");
        minified.Should().Contain("\"name\"");
    }

    [Fact]
    public void Merge_PatchOverridesSource()
    {
        var source = """{"a":1,"b":2}""";
        var patch  = """{"b":99,"c":3}""";
        var merged = JsonHelper.Merge(source, patch);

        JsonHelper.TryDeserialize<Dictionary<string, int>>(merged, out var result);
        result.Should().ContainKey("a").WhoseValue.Should().Be(1);
        result.Should().ContainKey("b").WhoseValue.Should().Be(99);
        result.Should().ContainKey("c").WhoseValue.Should().Be(3);
    }

    [Fact]
    public void Merge_NonObjectSource_Throws()
    {
        var act = () => JsonHelper.Merge("[1,2,3]", """{"a":1}""");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SerializeCamelCase_ProducesCamelCaseKeys()
    {
        var json = JsonHelper.SerializeCamelCase(new { FirstName = "Ana", LastName = "Lima" });
        json.Should().Contain("firstName");
        json.Should().Contain("lastName");
    }

    [Fact]
    public void TryGetProperty_ExistingKey_ReturnsTrueAndValue()
    {
        var json = """{"city":"São Paulo","country":"Brazil"}""";
        var found = JsonHelper.TryGetProperty(json, "city", out var value);

        found.Should().BeTrue();
        value.Should().Be("São Paulo");
    }

    [Fact]
    public void TryGetProperty_MissingKey_ReturnsFalse()
    {
        var json = """{"a":1}""";
        JsonHelper.TryGetProperty(json, "z", out _).Should().BeFalse();
    }

    [Fact]
    public void Flatten_NestedObject_ReturnsDotSeparatedKeys()
    {
        var json = """{"user":{"name":"Carlos","address":{"city":"Rio"}}}""";
        var flat = JsonHelper.Flatten(json);

        flat.Should().ContainKey("user.name").WhoseValue.Should().Be("Carlos");
        flat.Should().ContainKey("user.address.city").WhoseValue.Should().Be("Rio");
    }

    [Fact]
    public void Flatten_FlatObject_ReturnsSameKeys()
    {
        var json = """{"x":"1","y":"2"}""";
        var flat = JsonHelper.Flatten(json);

        flat.Should().ContainKey("x");
        flat.Should().ContainKey("y");
    }
}
