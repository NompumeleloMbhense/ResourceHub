using System.Net.Http.Json;
using Blazored.LocalStorage;
using ResourceHub.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace ResourceHub.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authProvider;

        public AuthService(IHttpClientFactory factory, ILocalStorageService localStorage,
            AuthenticationStateProvider authProvider)
        {
            _http = factory.CreateClient("ApiClient");
            _localStorage = localStorage;
            _authProvider = authProvider;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorage.SetItemAsync("authToken", result!.Token);

            if (_authProvider is CustomAuthStateProvider customProvider)
            {
                customProvider.NotifyUserLoggedIn();
            }

            return result?.Token;
        }

        public async Task<string?> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorage.SetItemAsync("authToken", result!.Token);

            if (_authProvider is CustomAuthStateProvider customProvider)
            {
                customProvider.NotifyUserLoggedIn();
            }

            return result?.Token;
        }


        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");

            if (_authProvider is CustomAuthStateProvider customProvider)
            {
                customProvider.NotifyUserLoggedOut();
            }
        }

        private class AuthResponse
        {
            public string Token { get; set; } = "";
        }
    }
}