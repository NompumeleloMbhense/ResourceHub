using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<UserDto>> GetUsersAsync(UserQueryParams queryParams)
        {
            var pagedUsers = await _repository.GetUsersAsync(queryParams);

            return new PagedResult<UserDto>
            {
                Data = pagedUsers.Data.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                }),

                PageNumber = pagedUsers.PageNumber,
                PageSize = pagedUsers.PageSize,
                TotalCount = pagedUsers.TotalCount,
                TotalPages = pagedUsers.TotalPages
            };
        }
    }
}