using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

/// <summary>
/// Custom authentication state provider that reads the JWT token 
/// from local storage and creates a ClaimsPrincipal based on it.
/// This allows the application to determibe if the user is authenticated
/// and what claims they have. It also provides methods to notify the app
/// when the user logs in or out so that the UI can update accordingly.
/// </summary>


namespace ResourceHub.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

                return new AuthenticationState(anonymous);
            }

            var claims = ParseClaimsFromJwt(token);

            var identity = new ClaimsIdentity(claims, "jwt");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotifyUserLoggedIn()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(anonymous)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims;
        }
    }
}