using FluentAssertions;

namespace Helpers.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void HasData_ShouldReturnTrue_WhenCollectionIsNotNullAndNotEmpty()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var result = list.HasData();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasData_ShouldReturnFalse_WhenCollectionIsNull()
    {
        // Arrange
        List<int>? list = null;

        // Act
        var result = list.HasData();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasData_ShouldReturnFalse_WhenCollectionIsEmpty()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var result = list.HasData();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IndexOf_ShouldReturnCorrectIndex_WhenValueExists()
    {
        // Arrange
        var list = new List<string> { "a", "b", "c" };

        // Act
        var index = list.IndexOf("b");

        // Assert
        index.Should().Be(1);
    }

    [Fact]
    public void IndexOf_ShouldReturnMinusOne_WhenValueDoesNotExist()
    {
        // Arrange
        var list = new List<string> { "a", "b", "c" };

        // Act
        var index = list.IndexOf("d");

        // Assert
        index.Should().Be(-1);
    }

    [Fact]
    public void FindItemWithNeighbors_ShouldReturnCorrectNeighbors_WhenItemExists()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = list.FindItemWithNeighbors(x => x == 3);

        // Assert
        result.Previous.Should().Be(2);
        result.Current.Should().Be(3);
        result.Next.Should().Be(4);
    }

    [Fact]
    public void FindItemWithNeighbors_ShouldReturnDefaults_WhenItemDoesNotExist()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = list.FindItemWithNeighbors(x => x == 6);

        // Assert
        result.Previous.Should().Be(default(int));
        result.Current.Should().Be(default(int));
        result.Next.Should().Be(default(int));
    }

    [Fact]
    public void IsEmpty_ShouldReturnTrue_WhenCollectionIsEmpty()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var result = list.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_ShouldReturnFalse_WhenCollectionIsNotEmpty()
    {
        // Arrange
        var list = new List<int> { 1 };

        // Act
        var result = list.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ForEach_ShouldApplyActionToAllElements()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var results = new List<int>();

        // Act
        list.ForEach(x => results.Add(x * 2));

        // Assert
        results.Should().ContainInOrder(2, 4, 6);
    }

    [Fact]
    public void ToCommaSeparatedString_ShouldReturnCommaSeparatedValues()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var result = list.ToCommaSeparatedString();

        // Assert
        result.Should().Be("1, 2, 3");
    }

    [Fact]
    public void ChunkBy_ShouldSplitCollectionIntoChunks()
    {
        // Arrange
        var list = Enumerable.Range(1, 10);

        // Act
        var chunks = list.ChunkBy(3).ToList();

        // Assert
        chunks.Should().HaveCount(4);
        chunks[0].Should().ContainInOrder(1, 2, 3);
        chunks[3].Should().ContainInOrder(10);
    }

    [Fact]
    public void ChunkBy_ShouldThrowException_WhenSizeIsLessThanOrEqualToZero()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        Action act = () => list.ChunkBy(0).ToList();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Chunk size must be greater than zero.*");
    }

    [Fact]
    public void DistinctBy_ShouldReturnDistinctElementsBasedOnKey()
    {
        // Arrange
        var list = new List<(int id, string name)> { (1, "A"), (2, "B"), (1, "C") };

        // Act
        var result = list.DistinctBy(x => x.id).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(x => x.name == "A");
        result.Should().Contain(x => x.name == "B");
    }

    [Fact]
    public void Shuffle_ShouldReturnAllElementsInRandomOrder()
    {
        // Arrange
        var list = Enumerable.Range(1, 10).ToList();

        // Act
        var shuffledList = list.Shuffle().ToList();

        // Assert
        shuffledList.Should().HaveCount(10);
        shuffledList.Should().Contain(list);
        shuffledList.Should().NotBeInAscendingOrder();
    }

    [Fact]
    public void Partition_ShouldSplitCollectionBasedOnPredicate()
    {
        // Arrange
        var list = Enumerable.Range(1, 10);

        // Act
        var (evens, odds) = list.Partition(x => x % 2 == 0);

        // Assert
        evens.Should().ContainInOrder(2, 4, 6, 8, 10);
        odds.Should().ContainInOrder(1, 3, 5, 7, 9);
    }
}
