using System;
using System.IO;
using Xunit;
using Helpers;
using FluentAssertions;

namespace Helpers.Tests
{
    public class FileHelperTests : IDisposable
    {
        private readonly string _tempDirectory;

        public FileHelperTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        [Fact]
        public void RemoveIllegalFileNameChars_ShouldRemoveIllegalCharacters()
        {
            // Arrange
            string input = "invalid<>:\\/|?*filename";
            string expected = "invalidfilename";

            // Act
            string result = FileHelper.RemoveIllegalFileNameChars(input);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void RemoveIllegalFileNameChars_ShouldReplaceIllegalCharactersWithSpecifiedReplacement()
        {
            // Arrange
            string input = "invalid<>:\\/|?*filename";
            string replacement = "_";
            string expected = "invalid________filename";

            // Act
            string result = FileHelper.RemoveIllegalFileNameChars(input, replacement);

            // Assert
            result.Should().Be(expected);
        }




        [Fact]
        public void ReadAllLines_ShouldReadFileLines()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            string[] lines = { "Line1", "Line2", "Line3" };
            FileHelper.WriteAllLines(filePath, lines);

            // Act
            string[] result = FileHelper.ReadAllLines(filePath);

            // Assert
            result.Should().BeEquivalentTo(lines);
        }

        [Fact]
        public void WriteAllLines_ShouldWriteFileLines()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            string[] lines = { "Line1", "Line2" };

            // Act
            FileHelper.WriteAllLines(filePath, lines);
            string[] result = File.ReadAllLines(filePath);

            // Assert
            result.Should().BeEquivalentTo(lines);
        }

        [Fact]
        public void ReadAllText_ShouldReadFileText()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            string text = "This is a test.";
            FileHelper.WriteAllText(filePath, text);

            // Act
            string result = FileHelper.ReadAllText(filePath);

            // Assert
            result.Should().Be(text);
        }

        [Fact]
        public void WriteAllText_ShouldWriteFileText()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            string text = "This is a test.";

            // Act
            FileHelper.WriteAllText(filePath, text);
            string result = File.ReadAllText(filePath);

            // Assert
            result.Should().Be(text);
        }

        [Fact]
        public void AppendAllText_ShouldAppendTextToFile()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            string initialText = "Initial text.";
            string appendText = " Appended text.";
            FileHelper.WriteAllText(filePath, initialText);

            // Act
            FileHelper.AppendAllText(filePath, appendText);
            string result = FileHelper.ReadAllText(filePath);

            // Assert
            result.Should().Be(initialText + appendText);
        }

        [Fact]
        public void FileExists_ShouldReturnTrueIfFileExists()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            FileHelper.WriteAllText(filePath, "Test");

            // Act
            bool exists = FileHelper.FileExists(filePath);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public void FileExists_ShouldReturnFalseIfFileDoesNotExist()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "nonexistent.txt");

            // Act
            bool exists = FileHelper.FileExists(filePath);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void DeleteFile_ShouldDeleteFile()
        {
            // Arrange
            string filePath = Path.Combine(_tempDirectory, "test.txt");
            FileHelper.WriteAllText(filePath, "Test");

            // Act
            FileHelper.DeleteFile(filePath);
            bool exists = FileHelper.FileExists(filePath);

            // Assert
            exists.Should().BeFalse();
        }

        [Fact]
        public void CopyFile_ShouldCopyFileToDestination()
        {
            // Arrange
            string sourceFilePath = Path.Combine(_tempDirectory, "source.txt");
            string destFilePath = Path.Combine(_tempDirectory, "dest.txt");
            FileHelper.WriteAllText(sourceFilePath, "This is a test.");

            // Act
            FileHelper.CopyFile(sourceFilePath, destFilePath);
            bool destExists = FileHelper.FileExists(destFilePath);
            string destContent = FileHelper.ReadAllText(destFilePath);

            // Assert
            destExists.Should().BeTrue();
            destContent.Should().Be("This is a test.");
        }

        [Fact]
        public void MoveFile_ShouldMoveFileToDestination()
        {
            // Arrange
            string sourceFilePath = Path.Combine(_tempDirectory, "source.txt");
            string destFilePath = Path.Combine(_tempDirectory, "dest.txt");
            FileHelper.WriteAllText(sourceFilePath, "This is a test.");

            // Act
            FileHelper.MoveFile(sourceFilePath, destFilePath);
            bool sourceExists = FileHelper.FileExists(sourceFilePath);
            bool destExists = FileHelper.FileExists(destFilePath);
            string destContent = FileHelper.ReadAllText(destFilePath);

            // Assert
            sourceExists.Should().BeFalse();
            destExists.Should().BeTrue();
            destContent.Should().Be("This is a test.");
        }


        /// <summary>
        /// Dispose method to clean up the temporary directory after tests.
        /// </summary>
        public void Dispose()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
    }
}

