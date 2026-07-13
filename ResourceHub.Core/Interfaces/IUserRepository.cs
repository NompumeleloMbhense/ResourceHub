using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}