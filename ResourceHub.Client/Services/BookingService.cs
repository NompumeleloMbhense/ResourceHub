using System.Net.Http.Json;
using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _http;

        public BookingService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<PagedResult<BookingDto>?> GetBookingsAsync(BookingQueryParams query)
        {
            var url = $"api/bookings?pageNumber={query.PageNumber}&pageSize={query.PageSize}";

            if (query.ResourceId.HasValue)
                url += $"&resourceId={query.ResourceId}";

            if (!string.IsNullOrWhiteSpace(query.BookedBy))
                url += $"&bookedBy={query.BookedBy}";

            if (!string.IsNullOrWhiteSpace(query.Search))
                url += $"&search={query.Search}";

            if (query.StartDate.HasValue)
                url += $"&startDate={query.StartDate:O}";

            if (query.EndDate.HasValue)
                url += $"&endDate={query.EndDate:O}";

            return await _http.GetFromJsonAsync<PagedResult<BookingDto>>(url);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<BookingDto>($"api/bookings/{id}");
        }

        public async Task<HttpResponseMessage> CreateBookingAsync(CreateBookingDto dto)
        {
            return await _http.PostAsJsonAsync("api/bookings", dto);
            
        }

        public async Task<bool> UpdateBookingAsync(int id, UpdateBookingDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/bookings/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/bookings/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}