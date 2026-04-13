using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Pagination;
using ResourceHub.Infrastructure.Persistence;

/// <summary>
/// Implements booking management logic, including conflict detection and resource 
/// availability checks. This service interacts with the database context to perform
/// CRUD operations on bookings
/// </summary>

namespace ResourceHub.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<PagedResult<Booking>> GetAllBookingsAsync(BookingQueryParams query)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Resource)
                .AsQueryable();

            // FILTERING
            if (query.ResourceId.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.ResourceId == query.ResourceId);

            if (!string.IsNullOrWhiteSpace(query.BookedBy))
                bookingsQuery = bookingsQuery.Where(b => b.BookedBy.Contains(query.BookedBy));

            if (query.StartDate.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.StartTime >= query.StartDate);

            if (query.EndDate.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.EndTime <= query.EndDate);


            var totalCount = await bookingsQuery.CountAsync();

            // PAGINATION
            var bookings = await bookingsQuery
                .OrderBy(b => b.StartTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Booking>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                Data = bookings
            };
        }


        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            // Ensure the resource exists
            var resource = await _context.Resources
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == booking.ResourceId);

            // Validate resource availability
            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            // Check if resource that is being booked is available
            if (!resource.IsAvailable)
                throw new ResourceUnavailableException("This resource is currently unavailable for booking");

            // Check for booking conflicts
            bool hasConflict = resource.Bookings.Any(b =>
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            // If there is a conflict, 
            // throw a custom exception that can be caught by the API layer to return 
            // a 409 conflict response 
            if (hasConflict)
                throw new BookingConflictException("This resource is already booked for the selected time slot");

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateBookingAsync(int bookingId, DateTime startTime, DateTime endTime,
                                                    string bookedBy, string purpose)
        {
            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");

            // Check conflicts excluding current booking
            bool hasConflict = booking.Resource.Bookings
                .Where(b => b.Id != bookingId)
                .Any(b => startTime < b.EndTime && endTime > b.StartTime);

            if (hasConflict)
                throw new BookingConflictException("This resource is already booked for the selected time slot");

            // Update fields
            booking.UpdateTime(startTime, endTime);
            booking.UpdateDetails(bookedBy, purpose);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");


            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }


        public async Task<PagedResult<Booking>> GetBookingByResourceAsync(int resourceId, BookingQueryParams query)
        {
            // BASE QUERY
            var bookingsQuery = _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.ResourceId == resourceId)
                .AsQueryable();


            // SEARCH
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();

                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookedBy.ToLower().Contains(search) ||
                    b.Purpose.ToLower().Contains(search) ||
                    b.Resource.Name.ToLower().Contains(search)
                );
            }

            // FILTERING
            if (query.ResourceId.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.ResourceId == query.ResourceId);

            if (!string.IsNullOrWhiteSpace(query.BookedBy))
                bookingsQuery = bookingsQuery.Where(b => b.BookedBy.Contains(query.BookedBy.ToLower()));

            if (query.StartDate.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.StartTime >= query.StartDate);

            if (query.EndDate.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.EndTime <= query.EndDate);


            var totalCount = await bookingsQuery.CountAsync();

            // PAGINATION
            var bookings = await bookingsQuery
                .OrderBy(b => b.StartTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Booking>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                Data = bookings
            };
        }
    }
}
