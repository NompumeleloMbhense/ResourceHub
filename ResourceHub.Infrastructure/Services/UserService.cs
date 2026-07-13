using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.DTOs;

namespace ResourceHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _repository.GetAllAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }).ToList();
        }
    }
}