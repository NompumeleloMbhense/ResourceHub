using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}