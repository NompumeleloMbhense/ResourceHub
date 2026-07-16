using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Core.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersAsync(UserQueryParams queryParams);
    }
}