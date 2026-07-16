using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<PagedResult<User>> GetUsersAsync(UserQueryParams queryParams)
        {
            var query = _context.Users.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim().ToLower();

                query = query.Where(u =>
                    u.Username.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search));
            }

            // Filter by role
            if (!string.IsNullOrWhiteSpace(queryParams.Role))
            {
                query = query.Where(u => u.Role == queryParams.Role);
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Username)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Data = users,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)queryParams.PageSize)
            };
        }
        

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail ||
                                                u.Email == usernameOrEmail);

        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}