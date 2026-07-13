using System.Net.Http.Json;
using ResourceHub.Shared.DTOs;

namespace ResourceHub.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<UserDto>>("api/users")
                   ?? new List<UserDto>();
        }
    }
}