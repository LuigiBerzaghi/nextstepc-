namespace NextStep.Application.Common;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Data { get; init; } = Array.Empty<T>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
}
