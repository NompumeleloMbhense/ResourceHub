using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Api.DTOs;
using AutoMapper;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        // GET: api/booking
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var result = _mapper.Map<IEnumerable<BookingDto>>(bookings);

            return Ok(result);
        }

        // GET: api/booking/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound();

            var result = _mapper.Map<BookingDto>(booking);

            return Ok(result);
        }

        // GET: api/booking/resource/1
        [HttpGet("resource/{resourceId}")]
        public async Task<IActionResult> GetByResource(int resourceId)
        {
            var bookings = await _bookingService.GetBookingByResourceAsync(resourceId);

            var result = _mapper.Map<IEnumerable<BookingDto>>(bookings);

            return Ok(result);
        }

        // POST: api/booking
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);

            await _bookingService.CreateBookingAsync(booking);

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, 
                                    _mapper.Map<BookingDto>(booking));
        }

        // PUT: api/booking/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto dto)
        {
            await _bookingService.UpdateBookingAsync(
                id,
                dto.StartTime,
                dto.EndTime,
                dto.BookedBy,
                dto.Purpose
            );

            return NoContent();
        }

        // DELETE: api/booking/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);

            return NoContent();
        }
    }
}
