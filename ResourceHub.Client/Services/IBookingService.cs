using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public interface IBookingService
    {
        Task<PagedResult<BookingDto>?> GetBookingsAsync(BookingQueryParams query);

        Task<BookingDto?> GetBookingByIdAsync(int id);

        Task<HttpResponseMessage> CreateBookingAsync(CreateBookingDto dto);

        Task<bool> UpdateBookingAsync(int id, UpdateBookingDto dto);

        Task<bool> DeleteBookingAsync(int id);
    }
}