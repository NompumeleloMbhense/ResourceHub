using ResourceHub.Core.Entities;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetUsersAsync(UserQueryParams queryParams);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}