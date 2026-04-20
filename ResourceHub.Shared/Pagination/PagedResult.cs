/// <summary>
/// Represents a paginated result set for any type of data
/// </summary>

namespace ResourceHub.Shared.Pagination
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}