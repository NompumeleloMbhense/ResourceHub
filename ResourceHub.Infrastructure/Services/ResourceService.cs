using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Pagination;
using ResourceHub.Infrastructure.Persistence;

/// <summary>
/// Implements resource management logic, including CRUD operations and pagination.
/// This service interacts with database context to perform operations on resources
/// </summary>

namespace ResourceHub.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {

        private readonly ApplicationDbContext _context;

        public ResourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Resource>> GetAllResourcesAsync(ResourceQueryParams query)
        {
            var resourcesQuery = _context.Resources
                .OrderBy(r => r.Id)
                .AsQueryable();


            // FILTERING
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                resourcesQuery = resourcesQuery
                    .Where(r => r.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Location))
            {
                resourcesQuery = resourcesQuery
                    .Where(r => r.Location.Contains(query.Location));
            }

            if (query.IsAvailable.HasValue)
            {
                resourcesQuery = resourcesQuery
                    .Where(r => r.IsAvailable == query.IsAvailable.Value);
            }

            if (query.MinCapacity.HasValue)
            {
                resourcesQuery = resourcesQuery
                    .Where(r => r.Capacity >= query.MinCapacity.Value);
            }

            if (query.MaxCapacity.HasValue)
            {
                resourcesQuery = resourcesQuery
                    .Where(r => r.Capacity <= query.MaxCapacity.Value);
            }

            var totalCount = await resourcesQuery.CountAsync();

            // PAGINATION
            var items = await resourcesQuery
                .OrderBy(r => r.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Resource>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize),
                Data = items
            };
        }

        public async Task<Resource?> GetResourceByIdAsync(int id)
        {
            return await _context.Resources.FindAsync(id);
        }

        public async Task<Resource> CreateResourceAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return resource;
        }

        public async Task UpdateResourceAsync(int id, string name, string description, string location, int capacity, bool isAvailable)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            // Update fields
            resource.UpdateDetails(name, description, location, capacity, isAvailable);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteResourceAsync(int id)
        {
            var resource = await _context.Resources
            .Include(r => r.Bookings)
            .FirstOrDefaultAsync(r => r.Id == id);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            if (resource.Bookings.Any())
                throw new ResourceHasBookingsException("Cannot delete a resource with existing bookings");

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }
    }
}