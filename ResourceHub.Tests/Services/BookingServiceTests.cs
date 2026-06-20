using Moq;
using Xunit;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.Pagination;
using ResourceHub.Infrastructure.Services;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Core.Exceptions;

namespace ResourceHub.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IResourceRepository> _resourceRepositoryMock;

        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _resourceRepositoryMock = new Mock<IResourceRepository>();
            _service = new BookingService(_bookingRepositoryMock.Object, _resourceRepositoryMock.Object);
        }


        // Test that creating a booking with overlapping time slot throws BookingConflictException
        [Fact]
        public async Task CreateBookingAsync_ShouldThrowConflictException_WhenTimeSlotOverlaps()
        {
            // Arrange
            var resource = new Resource
            (
                "Meeting Room",
                "Boardroom",
                "Floor 1",
                10
            );

            // (EF / tests only)
            typeof(Resource)
                .GetProperty("Id")!
                .SetValue(resource, 1);

            var existingBooking = new Booking
            (
                1,
                new DateTime(2026, 1, 1, 9, 0, 0),
                new DateTime(2026, 1, 1, 10, 0, 0),
                "Nompumelelo",
                "Team Meeting"
            );

            typeof(Booking)
                .GetProperty("Id")!
                .SetValue(existingBooking, 1);

            var newBooking = new Booking(
                1,
                new DateTime(2026, 1, 1, 9, 30, 0),
                new DateTime(2026, 1, 1, 10, 30, 0),
                "Jane",
                "Overlap Test"
            );


            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(resource);

            _bookingRepositoryMock
                .Setup(r => r.GetByResourceAsync(
                    1,
                    It.IsAny<BookingQueryParams>()))
                .ReturnsAsync(
                    new PagedResult<Booking>
                    {
                        Data = new List<Booking>
                        {
                    existingBooking
                        }
                    });


            // Act + Assert
            await Assert.ThrowsAsync<BookingConflictException>(
                () => _service.CreateBookingAsync(newBooking));
        }


        // Test that creating a booking with valid details saves the booking successfully
        [Fact]
        public async Task CreateBookingAsync_ShouldSaveBooking_WhenValid()
        {
            // Arrange
            var resource = new Resource
            (
                name: "Meeting Room 1",
                description: "Main boardroom",
                location: "Block A",
                capacity: 10
            );

            var booking = new Booking
            (
                resourceId: 1,
                startTime: DateTime.Now.AddHours(1),
                endTime: DateTime.Now.AddHours(2),
                bookedBy: "John Doe",
                purpose: "Team Meeting"
            );

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(resource);

            _bookingRepositoryMock
                .Setup(r => r.GetByResourceAsync(
                    1,
                    It.IsAny<BookingQueryParams>()))
                .ReturnsAsync(
                    new PagedResult<Booking>
                    {
                        Data = new List<Booking>()
                    });


            // Act
            await _service.CreateBookingAsync(booking);


            // Assert
            _bookingRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Booking>()),
                Times.Once);

            _bookingRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }


        // Test that creating a booking for a non-existent reource throws
        // ResourceNotFoundException
        [Fact]
        public async Task CreateBookingAsync_ShouldThrow_WhenResourceNotFound()
        {
            // Arrange
            var booking = new Booking
            (
                resourceId: 99,
                startTime: DateTime.Now.AddHours(1),
                endTime: DateTime.Now.AddHours(2),
                bookedBy: "John",
                purpose: "Meeting"
            );

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Resource?)null);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
                _service.CreateBookingAsync(booking));
        }


        // Test that creating a booking for an unvailable resource throws
        // ResourceUnavailableException
        [Fact]
        public async Task CreateBookingAsync_ShouldThrow_WhenResourceUnavailable()
        {
            // Arrange
            var resource = new Resource(
                "Room 1",
                "Desc",
                "Location",
                10
            );

            resource.SetAvailability(false);

            var booking = new Booking(
                1,
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                "John",
                "Meeting"
            );

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(resource);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceUnavailableException>(() =>
                _service.CreateBookingAsync(booking));
        }


        // Test that creating a booking with overlapping time slot throws BookingConflicException
        [Fact]
        public async Task CreateBookingAsync_ShouldThrow_WhenTimeConflictExists()
        {
            // Arrange
            var resource = new Resource(
                "Room 1",
                "Desc",
                "Location",
                10
            );

            var existingBooking = new Booking(
                1,
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(3),
                "Alice",
                "Existing Meeting"
            );

            var newBooking = new Booking(
                1,
                DateTime.Now.AddHours(2), // overlaps
                DateTime.Now.AddHours(4),
                "John",
                "New Meeting"
            );

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(resource);

            _bookingRepositoryMock
                .Setup(r => r.GetByResourceAsync(
                    1,
                    It.IsAny<BookingQueryParams>()))
                .ReturnsAsync(new PagedResult<Booking>
                {
                    Data = new List<Booking> { existingBooking }
                });

            // Act + Assert
            await Assert.ThrowsAsync<BookingConflictException>(() =>
                _service.CreateBookingAsync(newBooking));
        }


        // Test that moving a booking to a different resource updates the booking's ResourceId
        [Fact]
        public async Task MoveBookingAsync_ShouldUpdateResource_WhenValid()
        {
            // Arrange
            var booking = new Booking(
                1,
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                "John",
                "Meeting"
            );

            var resource = new Resource("Room 1", "Desc", "Loc", 10);

            _bookingRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(booking);

            _resourceRepositoryMock
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(resource);

            _bookingRepositoryMock
                .Setup(r => r.GetByResourceAsync(
                    2,
                    It.IsAny<BookingQueryParams>()))
                .ReturnsAsync(new PagedResult<Booking>
                {
                    Data = new List<Booking>()
                });

            // Act
            await _service.MoveBookingAsync(1, 2);

            // Assert
            _bookingRepositoryMock.Verify(r => r.Update(It.IsAny<Booking>()), Times.Once);
            _bookingRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}