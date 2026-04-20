using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;
using System.Net.Http.Json;

namespace ResourceHub.Client.Services
{
    public class ResourceService : IResourceService
    {
        private readonly HttpClient _http;

        public ResourceService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<PagedResult<ResourceDto>?> GetResourcesAsync(ResourceQueryParams query)
        {
            var url = $"api/resources?pageNumber={query.PageNumber}&pageSize={query.PageSize}";

            // Optional filters
            if (!string.IsNullOrWhiteSpace(query.Name))
                url += $"&name={query.Name}";

            if (!string.IsNullOrWhiteSpace(query.Location))
                url += $"&location={query.Location}";

            if (query.IsAvailable.HasValue)
                url += $"&isAvailable={query.IsAvailable}";

            if (query.MinCapacity.HasValue)
                url += $"&minCapacity={query.MinCapacity}";

            if (query.MaxCapacity.HasValue)
                url += $"&maxCapacity={query.MaxCapacity}";

            return await _http.GetFromJsonAsync<PagedResult<ResourceDto>>(url);
        }

        public async Task<ResourceDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ResourceDto>($"api/resources/{id}");
        }


        public async Task<bool> CreateAsync(CreateResourceDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/resources", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateResourceDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/resources/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/resources/{id}");
            return response.IsSuccessStatusCode;
        }

    }
}