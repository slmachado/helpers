using FluentAssertions;
using Xunit;

namespace Helpers.Tests
{
    public class BrasilDocsValidationHelperTests
    {
        [Theory]
        [InlineData("12345-678", true)]
        [InlineData("12345678", true)]
        [InlineData("00000-000", true)]
        [InlineData("1234", false)]
        public void IsValidCEP_ShouldValidateCorrectly(string cep, bool expected)
        {
            // Act
            bool result = BrasilDocsValidationHelper.IsValidCEP(cep);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("11444777000161", true)]
        [InlineData("00000000000000", false)] // Verificação de dígitos repetidos
        [InlineData("abcdefghijk123", false)] // Caracteres inválidos
        [InlineData("12.345.678/0001-95", true)] // CPF incorreto como CNPJ
        public void IsValidCNPJ_ShouldValidateCorrectly(string cnpj, bool expected)
        {
            // Act
            bool result = BrasilDocsValidationHelper.IsValidCNPJ(cnpj);

            // Assert
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("123.45678.90-1", false)] // PIS com máscara, considerado inválido
        [InlineData("12345678901", false)] // PIS inválido sem máscara
        [InlineData("00000000000", false)] // Verificação de dígitos repetidos
        [InlineData("abcdefghijk", false)] // Caracteres inválidos
        public void IsValidPIS_ShouldValidateCorrectly(string pis, bool expected)
        {
            // Act
            bool result = BrasilDocsValidationHelper.IsValidPIS(pis);

            // Assert
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("123.456.789-09", true)]
        [InlineData("00000000000", false)] // Verificação de dígitos repetidos
        [InlineData("12345678900", false)]
        [InlineData("12345678909", true)]
        [InlineData("abcdefghijk", false)] // Caracteres inválidos
        [InlineData("12.345.678/0001-95", false)] // CNPJ incorreto como CPF
        public void IsValidCPF_ShouldValidateCorrectly(string cpf, bool expected)
        {
            // Act
            bool result = BrasilDocsValidationHelper.IsValidCPF(cpf);

            // Assert
            result.Should().Be(expected);
        }
    }
}
