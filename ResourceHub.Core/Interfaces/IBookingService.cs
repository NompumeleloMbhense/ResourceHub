using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IBookingService
    {
        
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingByResourceAsync(int resourceId);
        Task CreateBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(int bookingId, DateTime startTime, DateTime endTime, string bookedBy, string purpose);
        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
