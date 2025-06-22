namespace Core.Shared;

public class QueryOptions
{
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public string? OrderBy { get; private set; }
    public bool OrderByDescending { get; private set; }
    
    public QueryOptions(int pageNumber = 1, int pageSize = 10, string? orderBy = null, bool orderByDescending = false)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);
        OrderBy = orderBy;
        OrderByDescending = orderByDescending;
    }

    public void Normalize()
    {
        PageNumber = PageNumber < 1 ? 1 : PageNumber;
        PageSize = PageSize < 1 ? 10 : (PageSize > 100 ? 100 : PageSize);
    }
}
