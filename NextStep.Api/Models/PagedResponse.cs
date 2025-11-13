namespace NextStep.Api.Models;

public class PagedResponse<T>
{
    public IReadOnlyCollection<T> Data { get; init; } = Array.Empty<T>();
    public PaginationMetadata Pagination { get; init; } = new();
    public IDictionary<string, string?> Links { get; init; } = new Dictionary<string, string?>();
}

public class PaginationMetadata
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
}
