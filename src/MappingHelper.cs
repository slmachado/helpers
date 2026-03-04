using System.Reflection;

namespace Helpers;

/// <summary>
/// A lightweight object mapper for simple DTO transformations.
/// </summary>
public static class MappingHelper
{
    /// <summary>
    /// Maps properties from a source object to a new instance of a destination type.
    /// Properties are matched by name and must have compatible types.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns>A new instance of TDestination with mapped properties.</returns>
    public static TDestination Map<TDestination>(object source) where TDestination : new()
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var destination = new TDestination();
        Map(source, destination);
        return destination;
    }

    /// <summary>
    /// Maps properties from a source object to an existing destination object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    public static void Map(object source, object destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));

        var sourceProps = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead);
        
        var destType = destination.GetType();

        foreach (var sourceProp in sourceProps)
        {
            var destProp = destType.GetProperty(sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);
            
            if (destProp != null && destProp.CanWrite && destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
            {
                var value = sourceProp.GetValue(source);
                destProp.SetValue(destination, value);
            }
        }
    }

    /// <summary>
    /// Maps a collection of source objects to a list of destination objects.
    /// </summary>
    public static IEnumerable<TDestination> MapList<TDestination>(IEnumerable<object> sourceList) where TDestination : new()
    {
        if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
        return sourceList.Select(Map<TDestination>);
    }
}
