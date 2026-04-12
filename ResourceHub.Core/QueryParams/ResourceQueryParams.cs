/// <summary>
/// Defines query parameters for fetching paginated list of resources
/// </summary>

namespace ResourceHub.Core.QueryParams
{
    public class ResourceQueryParams
    {
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 50 ? 50 : value; // max limit
        }

        // FILTERS
        public string? Name { get; set; }
        public string? Location { get; set; }
        public bool? IsAvailable { get; set; }
        public int? MinCapacity { get; set; }
        public int? MaxCapacity { get; set; }

        // SEARCH
        public string? Search { get; set; }
    }
}