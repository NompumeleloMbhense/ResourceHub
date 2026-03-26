using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Api.DTOs;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/booking
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            var result = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                ResourceId = b.ResourceId,
                ResourceName = b.Resource.Name,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                BookedBy = b.BookedBy,
                Purpose = b.Purpose
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound();

            var result = new BookingDto
            {
                Id = booking.Id,
                ResourceId = booking.ResourceId,
                ResourceName = booking.Resource.Name,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                BookedBy = booking.BookedBy,
                Purpose = booking.Purpose
            };

            return Ok(result);
        }

        // 
        // GET: api/booking/resource/1
        [HttpGet("resource/{resourceId}")]
        public async Task<IActionResult> GetByResource(int resourceId)
        {
            var bookings = await _bookingService.GetBookingByResourceAsync(resourceId);

            var result = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                ResourceId = b.ResourceId,
                ResourceName = b.Resource.Name,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                BookedBy = b.BookedBy,
                Purpose = b.Purpose
            });

            return Ok(result);
        }

        // POST: api/booking
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = new Booking(
            dto.ResourceId,
            dto.StartTime,
            dto.EndTime,
            dto.BookedBy,
            dto.Purpose
            );

            var success = await _bookingService.CreateBookingAsync(booking);

            if (!success)
                return BadRequest("Booking conflict or resource unavailable");

            return Ok("Booking created successfully");
        }

        // PUT: api/booking/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _bookingService.UpdateBookingAsync(
                id,
                dto.StartTime,
                dto.EndTime,
                dto.BookedBy,
                dto.Purpose
            );

            if (!success)
                return BadRequest("Update failed: conflict or booking not found.");

            return Ok("Booking updated successfully.");
        }

        // DELETE: api/booking/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _bookingService.DeleteBookingAsync(id);

            if (!success)
                return NotFound("Booking not found.");

            return Ok("Booking deleted successfully.");
        }
    }
}
