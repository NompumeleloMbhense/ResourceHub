using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Pagination;
using ResourceHub.Core.QueryParams;

/// <summary>
/// Service layer for managing bookings. Handles business logic and validation 
/// for booking operations, including creating, updating and deleting bookings
/// as well as retrieving booking information. Ensures that resources are available 
/// and that there are no scheduling conflicts when creating or updating bookings.
/// </summary>

namespace ResourceHub.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IResourceRepository _resourceRepository;

        public BookingService(IBookingRepository bookingRepository, IResourceRepository resourceRepository)
        {
            _bookingRepository = bookingRepository;
            _resourceRepository = resourceRepository;
        }

        public async Task<PagedResult<Booking>> GetAllBookingsAsync(BookingQueryParams query)
        {
            return await _bookingRepository.GetAllAsync(query);
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetByIdAsync(bookingId);
        }

        public async Task<PagedResult<Booking>> GetBookingByResourceAsync(int resourceId, BookingQueryParams query)
        {
            return await _bookingRepository.GetByResourceAsync(resourceId, query);
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            var resource = await _resourceRepository.GetByIdAsync(booking.ResourceId);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");


            if (!resource.IsAvailable)
                throw new ResourceUnavailableException("This resource is currently unavailable for booking");


            var existingBookings = await _bookingRepository.GetByResourceAsync(
                    booking.ResourceId,
                    new BookingQueryParams
                    {
                        PageNumber = 1,
                        PageSize = int.MaxValue
                    });

            bool hasConflict = existingBookings.Data.Any(b =>
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            if (hasConflict)
                throw new BookingConflictException("This resource is already booked for the selected time slot");


            await _bookingRepository.AddAsync(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(
            int bookingId,
            DateTime startTime,
            DateTime endTime,
            string bookedBy,
            string purpose)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");

            var existingBookings = await _bookingRepository.GetByResourceAsync(
                    booking.ResourceId,
                    new BookingQueryParams
                    {
                        PageNumber = 1,
                        PageSize = int.MaxValue
                    });

            bool hasConflict = existingBookings.Data
                .Where(b => b.Id != bookingId)
                .Any(b =>
                    startTime < b.EndTime &&
                    endTime > b.StartTime
                );

            if (hasConflict)
                throw new BookingConflictException("This resource is already booked for the selected time slot");


            booking.UpdateTime(startTime, endTime);

            booking.UpdateDetails(bookedBy, purpose);

            _bookingRepository.Update(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");


            _bookingRepository.Delete(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        // Move booking to a different resource
        // example: when a meeting room is unavailable, move the booking to another available
        // room without changing the time or other details
        public async Task MoveBookingAsync(int bookingId, int newResourceId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");

            var resource = await _resourceRepository.GetByIdAsync(newResourceId);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            if (!resource.IsAvailable)
                throw new ResourceUnavailableException("This resource is currently unavailable for booking");

            var existingBookings = _bookingRepository.GetByResourceAsync(
                newResourceId, new BookingQueryParams
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue
                });

            // Check for scheduling conflicts on the new resource
            bool hasConflict = existingBookings.Result.Data.Any(b =>
                b.Id != bookingId &&
                booking.StartTime < b.EndTime &&
                booking.EndTime > b.StartTime
            );

            if (hasConflict)
                throw new BookingConflictException(
                     "Target resource already booked for this time slot"
                );

            booking.MoveToResource(newResourceId);

            _bookingRepository.Update(booking);

            await _bookingRepository.SaveChangesAsync();
        }


    }
}