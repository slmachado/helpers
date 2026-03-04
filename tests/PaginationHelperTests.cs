using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class PaginationHelperTests
{
    [Fact]
    public void ToPagedResult_ShouldReturnCorrectSubset()
    {
        // Arrange
        var source = Enumerable.Range(1, 100);

        // Act
        var result = source.ToPagedResult(pageIndex: 2, pageSize: 10);

        // Assert
        result.Items.Should().HaveCount(10);
        result.Items.First().Should().Be(11);
        result.Items.Last().Should().Be(20);
        result.TotalCount.Should().Be(100);
        result.TotalPages.Should().Be(10);
        result.PageIndex.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void ToPagedResult_ShouldHandleLastPage()
    {
        // Arrange
        var source = Enumerable.Range(1, 25);

        // Act
        var result = source.ToPagedResult(pageIndex: 3, pageSize: 10);

        // Assert
        result.Items.Should().HaveCount(5);
        result.TotalPages.Should().Be(3);
        result.HasNextPage.Should().BeFalse();
    }
}
