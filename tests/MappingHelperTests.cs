using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class MappingHelperTests
{
    public class Source
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string OnlyInSource { get; set; } = "";
    }

    public class Destination
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string OnlyInDest { get; set; } = "";
    }

    [Fact]
    public void Map_ShouldCopyMatchingProperties()
    {
        // Arrange
        var source = new Source { Name = "John", Age = 30, OnlyInSource = "Secret" };

        // Act
        var dest = MappingHelper.Map<Destination>(source);

        // Assert
        dest.Name.Should().Be("John");
        dest.Age.Should().Be(30);
        dest.OnlyInDest.Should().BeEmpty();
    }

    [Fact]
    public void MapList_ShouldConvertCollection()
    {
        // Arrange
        var sourceList = new List<Source>
        {
            new() { Name = "A", Age = 10 },
            new() { Name = "B", Age = 20 }
        };

        // Act
        var destList = MappingHelper.MapList<Destination>(sourceList).ToList();

        // Assert
        destList.Should().HaveCount(2);
        destList[0].Name.Should().Be("A");
        destList[1].Name.Should().Be("B");
    }
}
