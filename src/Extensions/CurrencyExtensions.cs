using System.Globalization;

namespace Helpers.Extensions;

public static class CurrencyExtensions
{
    /// <summary>
    /// Formata um valor decimal como moeda com base em uma cultura específica.
    /// </summary>
    /// <param name="valor">Valor decimal a ser formatado</param>
    /// <param name="cultura">Cultura no formato "pt-BR", "en-US", etc.</param>
    /// <returns>Valor formatado como moeda</returns>
    public static string ToCurrency(this decimal valor, string cultura = "pt-BR")
    {
        var cultureInfo = new CultureInfo(cultura);
        return valor.ToString("C", cultureInfo);
    }
}
