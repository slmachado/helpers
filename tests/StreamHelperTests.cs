namespace Helpers.Tests;

using FluentAssertions;
using Helpers;
using System.IO;
using Xunit;

public class StreamHelperTests
{
    [Fact]
    public void CopyStream_ShouldCopyContentsCorrectly()
    {
        // Arrange
        byte[] inputData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        using var inputStream = new MemoryStream(inputData);
        using var outputStream = new MemoryStream();

        // Act
        StreamHelper.CopyStream(inputStream, outputStream);

        // Assert
        outputStream.ToArray().Should().BeEquivalentTo(inputData);
    }

    [Fact]
    public void CopyStream_ShouldLeaveInputStreamPositionUnchanged()
    {
        // Arrange
        byte[] inputData = new byte[] { 1, 2, 3, 4, 5 };
        using var inputStream = new MemoryStream(inputData);
        using var outputStream = new MemoryStream();
        long initialPosition = inputStream.Position;

        // Act
        StreamHelper.CopyStream(inputStream, outputStream);

        // Assert
        inputStream.Position.Should().Be(initialPosition);
        outputStream.ToArray().Should().BeEquivalentTo(inputData);
    }

    [Fact]
    public void CopyStream_ShouldCopyEmptyStreamWithoutError()
    {
        // Arrange
        using var inputStream = new MemoryStream();
        using var outputStream = new MemoryStream();

        // Act
        StreamHelper.CopyStream(inputStream, outputStream);

        // Assert
        outputStream.Length.Should().Be(0);
    }
}

