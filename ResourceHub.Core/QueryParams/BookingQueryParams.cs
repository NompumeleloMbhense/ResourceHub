/// <summary>
/// Defines query parameters for fetching paginated list of bookings
/// This class is used to encapsulate pagination parameters for 
/// booking-related queries, allowing clients to specify page number and 
/// page size 
/// </summary>

namespace ResourceHub.Core.QueryParams
{
    public class BookingQueryParams
    {
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 50 ? 50 : value;
        }

        // FILTERS
         public int? ResourceId { get; set; }
        public string? BookedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}