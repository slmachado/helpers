using System.Linq.Expressions;

namespace Helpers;

/// <summary>
/// Helper class for retrieving property names using expressions.
/// </summary>
public static class NameOfHelper
{
    /// <summary>
    /// Gets the name of a property from a given lambda expression.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TProp">The type of the property.</typeparam>
    /// <param name="obj">The object instance (can be null for static access).</param>
    /// <param name="propertyAccessor">An expression representing the property.</param>
    /// <returns>The name of the property as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided expression is not a valid member expression.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the property accessor is null.</exception>
    public static string GetPropertyName<T, TProp>(this T obj, Expression<Func<T, TProp>> propertyAccessor)
    {
        if (propertyAccessor == null)
            throw new ArgumentNullException(nameof(propertyAccessor), "Property accessor cannot be null.");

        if (propertyAccessor.Body is not MemberExpression body)
            throw new ArgumentException("The provided expression must be a member expression.", nameof(propertyAccessor));

        return body.Member.Name;
    }
}
