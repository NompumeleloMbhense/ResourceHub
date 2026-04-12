using ResourceHub.Core.Entities;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Pagination;

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
    }
}
