/// <summary>
/// Defines query parameters for fetching a paginated list of users.
/// </summary>

namespace ResourceHub.Shared.QueryParams
{
    public class UserQueryParams
    {
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 50 ? 50 : value;
        }

        // SEARCH
        public string? Search { get; set; }

        // FILTERS
        public string? Role { get; set; }
    }
}