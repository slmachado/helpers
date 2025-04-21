using System.Text.RegularExpressions;
// ReSharper disable InconsistentNaming

namespace Helpers
{
    /// <summary>
    /// Helper class for validating Brazilian documents such as CPF, CNPJ, PIS, and CEP.
    /// </summary>
    public static class BrasilDocsValidationHelper
    {
        private static readonly Regex OnlyDigitsRegex = new Regex(@"\D", RegexOptions.Compiled);

        /// <summary>
        /// Validates a Brazilian CEP (postal code).
        /// </summary>
        /// <param name="cep">The CEP to validate.</param>
        /// <returns>True if the CEP is valid; otherwise, false.</returns>
        public static bool IsValidCEP(string cep)
        {
            cep = NormalizeDocument(cep);
            return Regex.IsMatch(cep, @"^\d{8}$");
        }

        /// <summary>
        /// Validates a Brazilian CNPJ (Company Identifier).
        /// </summary>
        /// <param name="cnpj">The CNPJ to validate.</param>
        /// <returns>True if the CNPJ is valid; otherwise, false.</returns>
        public static bool IsValidCNPJ(string cnpj)
        {
            cnpj = NormalizeDocument(cnpj);

            if (cnpj.Length != 14 || IsRepeatedDigits(cnpj))
                return false;

            var tempCnpj = cnpj.Substring(0, 12);
            var digito = CalculateChecksum(tempCnpj, new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }).ToString();

            tempCnpj += digito;
            digito += CalculateChecksum(tempCnpj, new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }).ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Validates a Brazilian PIS (Social Integration Program).
        /// </summary>
        /// <param name="pis">The PIS to validate.</param>
        /// <returns>True if the PIS is valid; otherwise, false.</returns>
        public static bool IsValidPIS(string pis)
        {
            pis = NormalizeDocument(pis).PadLeft(11, '0');

            if (pis.Length != 11 || IsRepeatedDigits(pis))
                return false;

            var checksum = CalculateChecksum(pis.Substring(0, 10), new[] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
            var expectedDigit = (checksum % 11) < 2 ? 0 : 11 - (checksum % 11);

            return pis.EndsWith(expectedDigit.ToString());
        }

        /// <summary>
        /// Validates a Brazilian CPF (Individual Taxpayer Registry).
        /// </summary>
        /// <param name="cpf">The CPF to validate.</param>
        /// <returns>True if the CPF is valid; otherwise, false.</returns>
        public static bool IsValidCPF(string cpf)
        {
            cpf = NormalizeDocument(cpf);

            if (cpf.Length != 11 || IsRepeatedDigits(cpf))
                return false;

            var tempCpf = cpf[..9];
            var digito = CalculateChecksum(tempCpf, new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 }).ToString();

            tempCpf += digito;
            digito += CalculateChecksum(tempCpf, new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 }).ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Normalizes a document string by removing non-digit characters.
        /// </summary>
        /// <param name="document">The document string to normalize.</param>
        /// <returns>A string containing only digits.</returns>
        private static string NormalizeDocument(string document)
        {
            return OnlyDigitsRegex.Replace(document.Trim(), "");
        }

        /// <summary>
        /// Calculates the checksum for a given document using specified multipliers.
        /// </summary>
        /// <param name="input">The input string to calculate the checksum for.</param>
        /// <param name="multipliers">The multipliers to use in the checksum calculation.</param>
        /// <returns>The calculated checksum value.</returns>
        private static int CalculateChecksum(string input, int[] multipliers)
        {
            int sum = 0;
            for (int i = 0; i < multipliers.Length; i++)
            {
                sum += int.Parse(input[i].ToString()) * multipliers[i];
            }

            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        /// <summary>
        /// Checks if a document is composed of repeated digits.
        /// </summary>
        /// <param name="document">The document string to check.</param>
        /// <returns>True if all characters are the same; otherwise, false.</returns>
        private static bool IsRepeatedDigits(string document)
        {
            return document.Distinct().Count() == 1;
        }
    }
}
