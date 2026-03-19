using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;

namespace ResourceHub.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            // Ensure resource exists and is available
            var resource = await _context.Resources
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == booking.ResourceId);

            if (resource == null || !resource.IsAvailable)
                return false;

            // Check for booking conflicts
            bool hasConflict = resource.Bookings.Any(b =>
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            if (hasConflict)
                return false;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingByResourceAsync(int resourceId)
        {
            return await _context.Bookings
                .Where(b => b.ResourceId == resourceId)
                .ToListAsync();
        }

        public async Task<bool> UpdateBookingAsync(int bookingId, DateTime startTime, DateTime endTime,
                                                    string bookedBy, string purpose)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if(booking == null) 
                return false;

            // Check conflicts excluding current booking
            var hasConflict = await _context.Bookings
                .Where(b => b.ResourceId == booking.ResourceId && b.Id != bookingId)
                .AnyAsync(b => startTime < b.EndTime && endTime > b.StartTime);

            if(hasConflict)
                return false;

            // Update fields
            booking.UpdateTime(startTime, endTime);
            booking.UpdateDetails(bookedBy, purpose);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if(booking == null)
                return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }
        
    }
}
