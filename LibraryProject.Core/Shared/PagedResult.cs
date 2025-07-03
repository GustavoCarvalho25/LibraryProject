namespace Core.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    
    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PagedResult<TDestination> From<TSource, TDestination>(
        IEnumerable<TDestination> items,
        PagedResult<TSource> source)
    => new PagedResult<TDestination>(
        items,
        source.TotalCount,
        source.PageNumber,
        source.PageSize);
}
