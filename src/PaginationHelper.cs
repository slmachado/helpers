namespace Helpers;

/// <summary>
/// Provides extension methods and utilities for paginating collections.
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Paginates an IEnumerable collection.
    /// </summary>
    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
    {
        var list = source.ToList();
        var totalCount = list.Count;
        var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<T>(items, totalCount, pageIndex, pageSize);
    }

    /// <summary>
    /// Paginates an IQueryable (useful for Entity Framework).
    /// </summary>
    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        var totalCount = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<T>(items, totalCount, pageIndex, pageSize);
    }
}

/// <summary>
/// Represents a paginated result set.
/// </summary>
public class PagedResult<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PagedResult(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
