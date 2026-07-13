using ResourceHub.Shared.DTOs;

namespace ResourceHub.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
    }
}