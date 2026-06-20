using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Infrastructure.Persistence;

namespace ResourceHub.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Resource>> GetAllAsync(ResourceQueryParams query)
        {
            var resourcesQuery = _context.Resources
                .AsNoTracking()
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var name = query.Name.ToLower();

                resourcesQuery = resourcesQuery.Where(r =>
                    r.Name.ToLower().Contains(name));
            }

            // FILTERING
            if (!string.IsNullOrWhiteSpace(query.Location))
            {
                var location = query.Location.ToLower();

                resourcesQuery = resourcesQuery.Where(r =>
                    r.Location.ToLower().Contains(location));
            }

            if (query.IsAvailable.HasValue)
            {
                resourcesQuery = resourcesQuery.Where(r =>
                    r.IsAvailable == query.IsAvailable.Value);
            }

            if (query.MinCapacity.HasValue)
            {
                resourcesQuery = resourcesQuery.Where(r =>
                    r.Capacity >= query.MinCapacity.Value);
            }

            if (query.MaxCapacity.HasValue)
            {
                resourcesQuery = resourcesQuery.Where(r =>
                    r.Capacity <= query.MaxCapacity.Value);
            }

            var totalCount = await resourcesQuery.CountAsync();

            var resources = await resourcesQuery
                .OrderBy(r => r.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Resource>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize),
                Data = resources
            };
        }

        public async Task<Resource?> GetByIdAsync(int id)
        {
            return await _context.Resources
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
        }

        public void Update(Resource resource)
        {
            _context.Resources.Update(resource);
        }

        public void Delete(Resource resource)
        {
            _context.Resources.Remove(resource);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}