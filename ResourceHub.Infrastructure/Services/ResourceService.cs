using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;

namespace ResourceHub.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {

        private readonly ApplicationDbContext _context;

        public ResourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources.ToListAsync();
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

            if(resource == null)
                throw new ResourceNotFoundException("Resource not found");

            if(resource.Bookings.Any())
                throw new ResourceHasBookingsException("Cannot delete a resource with existing bookings");

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }
    }
}