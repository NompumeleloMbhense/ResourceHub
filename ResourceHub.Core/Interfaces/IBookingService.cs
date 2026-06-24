using ResourceHub.Core.Entities;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Shared.Pagination;

namespace ResourceHub.Core.Interfaces
{
    public interface IBookingService
    {

        Task<PagedResult<Booking>> GetAllBookingsAsync(BookingQueryParams query);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<PagedResult<Booking>> GetBookingByResourceAsync(int resourceId, BookingQueryParams query);
        Task CreateBookingAsync(Booking booking);
        Task UpdateBookingAsync(int bookingId, DateTime startTime, DateTime endTime, string bookedBy, string purpose);
        Task DeleteBookingAsync(int bookingId);
        Task MoveBookingAsync(int bookingId, int newResourceId);
        Task<bool> IsResourceAvailableAsync(int resourceId, DateTime start, DateTime end);
    }
}
