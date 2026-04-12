using ResourceHub.Core.Entities;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Pagination;

namespace ResourceHub.Core.Interfaces
{
    public interface IResourceService
    {
        Task<PagedResult<Resource>> GetAllResourcesAsync(ResourceQueryParams query);

        Task<Resource?> GetResourceByIdAsync(int id);

        Task<Resource> CreateResourceAsync(Resource resource);

        Task UpdateResourceAsync(int id, string name, string description, string location, int capacity, bool isAvailable);

        Task DeleteResourceAsync(int id);
    }
}