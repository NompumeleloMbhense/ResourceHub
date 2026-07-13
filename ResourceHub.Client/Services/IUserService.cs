using ResourceHub.Shared.DTOs;

namespace ResourceHub.Client.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
    }
}