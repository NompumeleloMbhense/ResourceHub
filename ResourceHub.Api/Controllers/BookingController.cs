using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Entities;
using ResourceHub.Core.QueryParams;
using ResourceHub.Api.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetAll([FromQuery] BookingQueryParams query)
        {
            var pagedBookings = await _bookingService.GetAllBookingsAsync(query);

            var result = new
            {
                pagedBookings.PageNumber,
                pagedBookings.PageSize,
                pagedBookings.TotalCount,
                pagedBookings.TotalPages,
                Data = _mapper.Map<IEnumerable<BookingDto>>(pagedBookings.Data)
            };

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
        public async Task<IActionResult> GetBookingByResource(int resourceId, [FromQuery] BookingQueryParams query)
        {
            var pagedBookings = await _bookingService.GetBookingByResourceAsync(resourceId, query);

            var result = new
            {
                pagedBookings.PageNumber,
                pagedBookings.PageSize,
                pagedBookings.TotalCount,
                pagedBookings.TotalPages,
                Data = _mapper.Map<IEnumerable<BookingDto>>(pagedBookings.Data)
            };

            return Ok(result);
        }

        // POST: api/booking
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);

            await _bookingService.CreateBookingAsync(booking);

            return CreatedAtAction(nameof(GetById), new { id = booking.Id },
                                    _mapper.Map<BookingDto>(booking));
        }

        // PUT: api/booking/1
        [Authorize]
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);

            return NoContent();
        }
    }
}
