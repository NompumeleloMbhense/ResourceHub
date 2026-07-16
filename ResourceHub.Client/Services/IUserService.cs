using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersAsync(UserQueryParams query);
    }
}