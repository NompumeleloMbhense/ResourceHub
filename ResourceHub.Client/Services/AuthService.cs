using System.Net.Http.Json;
using Blazored.LocalStorage;
using ResourceHub.Shared.DTOs;

namespace ResourceHub.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory factory, ILocalStorageService localStorage)
        {
            _http = factory.CreateClient("ApiClient");
            _localStorage = localStorage;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorage.SetItemAsync("authToken", result!.Token);

            return result?.Token;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        private class AuthResponse
        {
            public string Token { get; set; } = "";
        }
    }
}