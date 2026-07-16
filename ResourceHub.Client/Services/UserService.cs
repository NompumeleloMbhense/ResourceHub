using System.Net.Http.Json;
using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<PagedResult<UserDto>> GetUsersAsync(UserQueryParams query)
        {
            var url =
                $"api/users?" +
                $"Search={query.Search}" +
                $"&Role={query.Role}" +
                $"&PageNumber={query.PageNumber}" +
                $"&PageSize={query.PageSize}";

            return await _http.GetFromJsonAsync<PagedResult<UserDto>>(url)
                   ?? new PagedResult<UserDto>();
        }
    }
}