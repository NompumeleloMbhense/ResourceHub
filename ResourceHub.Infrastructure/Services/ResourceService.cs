using ResourceHub.Core.Entities;
using ResourceHub.Core.Exceptions;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Pagination;
using ResourceHub.Core.QueryParams;

/// <summary>
/// Service layer for managing reources. Handles business logic and validation 
/// for resource operations, including creating, updating and deleting resources
/// as well as retrieving resource information. Ensures that resources are valid.
/// </summary>

namespace ResourceHub.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public Task<PagedResult<Resource>> GetAllResourcesAsync(ResourceQueryParams query)
        {
            return _resourceRepository.GetAllAsync(query);
        }

        public Task<Resource?> GetResourceByIdAsync(int id)
        {
            return _resourceRepository.GetByIdAsync(id);
        }

        public async Task CreateResourceAsync(Resource resource)
        {
            await _resourceRepository.AddAsync(resource);
            await _resourceRepository.SaveChangesAsync();
        }

        public async Task UpdateResourceAsync(int id, Resource updated)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            resource.UpdateDetails(
                updated.Name,
                updated.Description,
                updated.Location,
                updated.Capacity,
                updated.IsAvailable
            );


            await _resourceRepository.SaveChangesAsync();
        }

        public async Task DeleteResourceAsync(int id)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);

            if (resource == null)
                throw new ResourceNotFoundException("Resource not found");

            if (resource.Bookings.Any())
                throw new ResourceHasBookingsException("Cannot delete resource with existing bookings");


            _resourceRepository.Delete(resource);
            await _resourceRepository.SaveChangesAsync();
        }
    }
}