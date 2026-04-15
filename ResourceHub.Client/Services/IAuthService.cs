using ResourceHub.Shared.DTOs;

namespace ResourceHub.Client.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}