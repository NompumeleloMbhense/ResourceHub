using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

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
            await GetAvailableResourceOrThrowAsync(booking.ResourceId);

            await ValidateBookingConflictAsync(
                    booking.ResourceId,
                    booking.StartTime,
                    booking.EndTime);

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
            var booking = await GetBookingOrThrowAsync(bookingId);

            await ValidateBookingConflictAsync(
                booking.ResourceId,
                startTime,
                endTime,
                bookingId);

            booking.UpdateTime(startTime, endTime);

            booking.UpdateDetails(bookedBy, purpose);

            _bookingRepository.Update(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await GetBookingOrThrowAsync(bookingId);

            _bookingRepository.Delete(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        // Move booking to a different resource
        // example: when a meeting room is unavailable, move the booking to another available
        // room without changing the time or other details
        public async Task MoveBookingAsync(int bookingId, int newResourceId)
        {
            var booking = await GetBookingOrThrowAsync(bookingId);

            await GetAvailableResourceOrThrowAsync(newResourceId);

            await ValidateBookingConflictAsync(
                    newResourceId,
                    booking.StartTime,
                    booking.EndTime,
                    bookingId);

            booking.MoveToResource(newResourceId);

            _bookingRepository.Update(booking);

            await _bookingRepository.SaveChangesAsync();
        }

        // Check if a resource is available for booking in a given time slot
        public async Task<bool> IsResourceAvailableAsync(int resourceId, DateTime start, DateTime end)
        {
            var bookings = await _bookingRepository.GetByResourceAsync(resourceId,
            new BookingQueryParams
            {
                PageSize = 1000
            });

            return !bookings.Data.Any(b =>
                b.StartTime < end &&
                b.EndTime > start
                );
        }


        // ------------------------ Private Helper Methods -----------------------------------------------//

        // Helper method to retrieve a booking or throw an exception if not found
        private async Task<Booking> GetBookingOrThrowAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException("Booking not found");

            return booking;
        }

        // Helper method to get a resource or throw an exception if not found or unavailable
        private async Task<Resource> GetAvailableResourceOrThrowAsync(int resourceId)
        {
            var resource = await _resourceRepository.GetByIdAsync(resourceId);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            if (!resource.IsAvailable)
                throw new ResourceUnavailableException(
                    "This resource is currently unavailable for booking");

            return resource;
        }

        // Helper method to check for booking conflicts for a given reource and 
        // time slot, excluding a specific booking ID (used for updates)
        private async Task ValidateBookingConflictAsync(
                int resourceId,
                DateTime startTime,
                DateTime endTime,
                int? excludeBookingId = null
        )
        {
            var existingBookings = await _bookingRepository.GetByResourceAsync(
                resourceId,
                new BookingQueryParams
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue
                });

            bool hasConflict = existingBookings.Data.Any(b =>
                    b.Id != excludeBookingId &&
                    startTime < b.EndTime &&
                    endTime > b.StartTime
                    );
            if (hasConflict)
                throw new BookingConflictException(
                    "This resource is already booked for the selected time slot");

        }


    }
}