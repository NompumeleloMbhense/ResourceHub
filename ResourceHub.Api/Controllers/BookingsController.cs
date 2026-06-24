using Microsoft.AspNetCore.Mvc;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Core.Entities;
using ResourceHub.Shared.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ResourceHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookingQueryParams query)
        {
            var result = await _bookingService.GetAllBookingsAsync(query);

            return Ok(new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalCount,
                result.TotalPages,
                Data = _mapper.Map<IEnumerable<BookingDto>>(result.Data)
            });
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound();

            return Ok(_mapper.Map<BookingDto>(booking));
        }

        // GET: api/bookings/resource/{resourceId}
        [HttpGet("resource/{resourceId}")]
        public async Task<IActionResult> GetByResource(int resourceId, [FromQuery] BookingQueryParams query)
        {
            var result = await _bookingService.GetBookingByResourceAsync(resourceId, query);

            return Ok(new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalCount,
                result.TotalPages,
                Data = _mapper.Map<IEnumerable<BookingDto>>(result.Data)
            });
        }

        // POST: api/bookings
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);

            await _bookingService.CreateBookingAsync(booking);

            return CreatedAtAction(
                nameof(GetById),
                new { id = booking.Id },
                _mapper.Map<BookingDto>(booking)
            );
        }

        // PUT: api/bookings/{id}
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


        // DELETE: api/bookings/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(int id, MoveBookingDto dto)
        {
            await _bookingService.MoveBookingAsync(id, dto.NewResourceId);

            return NoContent();
        }

        [HttpGet("resource/{resourceId}/upcoming")]
        public async Task<IActionResult> GetUpcomingBookingByResource(int resourceId)
        {
            var query = new BookingQueryParams
            {
                UpcomingOnly = true
            };

            var result = await _bookingService.GetBookingByResourceAsync(resourceId, query);

            return Ok(new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalCount,
                result.TotalPages,
                Data = _mapper.Map<IEnumerable<BookingDto>>(result.Data)
            });
        }

        [HttpGet("resource/{resourceId}/availability")]
        public async Task<IActionResult> CheckAvailability(int resourceId, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var IsAvailable = await _bookingService.IsResourceAvailableAsync(resourceId, start, end);

            return Ok(new { IsAvailable });
        }
    }
}