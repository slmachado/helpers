using System;
using Helpers;
using FluentAssertions;
using Xunit;

namespace Helpers.Tests
{
    public class Base64HelperTests
    {
        #region SafeConvertFromBase64String tests

        [Fact]
        public void SafeConvertFromBase64String_ValidBase64String_ShouldReturnDecodedByteArray()
        {
            string input = "SGVsbG8gd29ybGQ="; // "Hello world"
            byte[] result = Base64Helper.SafeConvertFromBase64String(input);
            result.Should().BeEquivalentTo(new byte[] { 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 });
        }

        [Fact]
        public void SafeConvertFromBase64String_UrlSafeBase64String_ShouldReturnDecodedByteArray()
        {
            string input = "SGVsbG8td29ybGQ="; // Versão padrão Base64 da string "Hello-world"
            byte[] result = Base64Helper.SafeConvertFromBase64String(input);

            // Verifique o comprimento do resultado
            result.Length.Should().Be(11, "porque o tamanho esperado é de 11 bytes para a string 'Hello-world'");

            // Verifique o valor exato do resultado
            result.Should().BeEquivalentTo(new byte[] { 72, 101, 108, 108, 111, 45, 119, 111, 114, 108, 100 });
        }

        [Fact]
        public void SafeConvertFromBase64String_InvalidBase64String_ShouldReturnEmptyByteArray()
        {
            string input = "InvalidBase64";
            byte[] result = Base64Helper.SafeConvertFromBase64String(input);
            result.Should().BeEmpty();
        }

        [Fact]
        public void SafeConvertFromBase64String_EmptyString_ShouldReturnEmptyByteArray()
        {
            string input = "";
            byte[] result = Base64Helper.SafeConvertFromBase64String(input);
            result.Should().BeEmpty();
        }

        [Fact]
        public void SafeConvertFromBase64String_NullString_ShouldReturnEmptyByteArray()
        {
            string? input = null;
            byte[] result = Base64Helper.SafeConvertFromBase64String(input);
            result.Should().BeEmpty();
        }

        #endregion

        #region EncodeToBase64String tests

        [Fact]
        public void EncodeToBase64String_ValidByteArray_ShouldReturnBase64String()
        {
            byte[] input = new byte[] { 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 }; // "Hello world"
            string result = Base64Helper.EncodeToBase64String(input);
            result.Should().Be("SGVsbG8gd29ybGQ=");
        }

        [Fact]
        public void EncodeToBase64String_EmptyByteArray_ShouldReturnEmptyString()
        {
            byte[] input = Array.Empty<byte>();
            string result = Base64Helper.EncodeToBase64String(input);
            result.Should().BeEmpty();
        }

        [Fact]
        public void EncodeToBase64String_NullByteArray_ShouldReturnEmptyString()
        {
            byte[]? input = null;
            string result = Base64Helper.EncodeToBase64String(input);
            result.Should().BeEmpty();
        }

        #endregion

        #region ConvertFromBase64String with throwOnInvalidInput tests

        [Fact]
        public void ConvertFromBase64String_ValidBase64String_ShouldReturnDecodedByteArray()
        {
            string input = "SGVsbG8gd29ybGQ="; // "Hello world"
            byte[] result = Base64Helper.ConvertFromBase64String(input, false);
            result.Should().BeEquivalentTo(new byte[] { 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 });
        }

        [Fact]
        public void ConvertFromBase64String_InvalidBase64String_ShouldReturnEmptyByteArray_WhenThrowOnInvalidInputIsFalse()
        {
            string input = "InvalidBase64";
            byte[] result = Base64Helper.ConvertFromBase64String(input, false);
            result.Should().BeEmpty();
        }

        [Fact]
        public void ConvertFromBase64String_InvalidBase64String_ShouldThrowFormatException_WhenThrowOnInvalidInputIsTrue()
        {
            string input = "InvalidBase64";
            Action act = () => Base64Helper.ConvertFromBase64String(input, true);
            act.Should().Throw<FormatException>();
        }

        [Fact]
        public void ConvertFromBase64String_EmptyString_ShouldReturnEmptyByteArray()
        {
            string input = "";
            byte[] result = Base64Helper.ConvertFromBase64String(input, false);
            result.Should().BeEmpty();
        }

        [Fact]
        public void ConvertFromBase64String_NullString_ShouldReturnEmptyByteArray()
        {
            string? input = null;
            byte[] result = Base64Helper.ConvertFromBase64String(input, false);
            result.Should().BeEmpty();
        }

        #endregion
    }
}