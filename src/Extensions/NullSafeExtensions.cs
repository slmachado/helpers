using System.Diagnostics.CodeAnalysis;

namespace Helpers.Extensions;

/// <summary>
/// Métodos de extensão para facilitar o tratamento de referências nulas,
/// melhorando a legibilidade e segurança de código com suporte a expressões elegantes.
/// </summary>
public static class NullSafeExtensions
{
    /// <summary>
    /// Lança uma exceção se o objeto for <c>null</c>. Caso contrário, retorna o próprio objeto com tipo não anulável.
    /// Útil para encadear chamadas sem precisar de verificações explícitas.
    /// </summary>
    /// <typeparam name="T">Tipo da referência a ser validada (restrito a tipos de referência).</typeparam>
    /// <param name="obj">Objeto a ser validado.</param>
    /// <param name="message">Mensagem da exceção a ser lançada, caso <paramref name="obj"/> seja <c>null</c>.</param>
    /// <returns>O próprio objeto com tipo não anulável.</returns>
    /// <exception cref="InvalidOperationException">Lançada quando <paramref name="obj"/> é <c>null</c>.</exception>
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? obj, string message = "Valor inesperadamente nulo") where T : class
    {
        return obj ?? throw new InvalidOperationException(message);
    }

    /// <summary>
    /// Retorna <c>null</c> se o predicado especificado retornar <c>true</c> para o objeto.
    /// Caso contrário, retorna o próprio objeto.
    /// </summary>
    /// <typeparam name="T">Tipo da referência analisada.</typeparam>
    /// <param name="obj">Objeto a ser testado.</param>
    /// <param name="predicate">Função booleana que define quando o objeto deve ser considerado nulo.</param>
    /// <returns><c>null</c> se o predicado retornar <c>true</c>; caso contrário, o próprio objeto.</returns>
    public static T? NullIf<T>(this T? obj, Func<T?, bool> predicate) where T : class
    {
        return predicate(obj) ? null : obj;
    }
    
    
    /// <summary>
    /// Lança uma exceção se a coleção for <c>null</c>, vazia ou composta apenas por espaços (no caso de <see cref="string"/>).
    /// Utiliza materialização com <c>ToList()</c> apenas quando necessário para evitar múltiplas enumerações.
    /// </summary>
    /// <typeparam name="T">Tipo dos elementos da coleção.</typeparam>
    /// <param name="source">Coleção a ser validada.</param>
    /// <param name="message">Mensagem da exceção a ser lançada se a coleção for inválida.</param>
    /// <returns>A própria coleção, caso contenha elementos válidos.</returns>
    /// <exception cref="InvalidOperationException">Lançada quando a coleção for nula, vazia ou inválida.</exception>
    public static IEnumerable<T> ThrowIfEmpty<T>(this IEnumerable<T>? source, string message = "Coleção inesperadamente vazia")
    {
        switch (source)
        {
            case null:
            case string str when string.IsNullOrWhiteSpace(str):
                throw new InvalidOperationException(message);
        }

        // Se já for uma coleção, use diretamente
        if (source is ICollection<T> collection)
        {
            if (collection.Count == 0)
                throw new InvalidOperationException(message);
            return collection;
        }

        // Caso contrário, materializa em uma lista e verifica
        var list = source.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException(message);

        return list;
    }

    /// <summary>
    /// Verifica se um valor está presente em uma lista de valores fornecida.
    /// </summary>
    public static bool IsIn<T>(this T value, params T[] values) where T : notnull
    {
        return values.Contains(value);
    }

    /// <summary>
    /// Retorna o primeiro elemento que satisfaça o predicado ou um valor alternativo fornecido.
    /// </summary>
    public static T SafeFirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback)
    {
        return source.FirstOrDefault(predicate) ?? fallback;
    }

    /// <summary>
    /// Executa uma ação se o objeto não for <c>null</c>.
    /// </summary>
    public static void IfNotNull<T>(this T? obj, Action<T> action) where T : class
    {
        if (obj != null)
            action(obj);
    }

    /// <summary>
    /// Executa uma transformação no objeto, se não for <c>null</c>, retornando o resultado. Caso contrário, retorna <c>null</c>.
    /// </summary>
    public static TResult? With<T, TResult>(this T? obj, Func<T, TResult> selector) where T : class
    {
        return obj != null ? selector(obj) : default;
    }
}
