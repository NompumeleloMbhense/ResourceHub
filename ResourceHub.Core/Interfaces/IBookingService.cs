using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CreateBookingAsync(Booking booking);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingByResourceAsync(int resourceId);
        Task<bool> UpdateBookingAsync(int bookingId, DateTime startTime, DateTime endTime, string bookedBy, string purpose);
        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
