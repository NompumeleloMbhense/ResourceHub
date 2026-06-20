using ResourceHub.Core.Entities;
using ResourceHub.Shared.QueryParams;
using ResourceHub.Shared.Pagination;

namespace ResourceHub.Core.Interfaces
{
    public interface IResourceService
    {
        Task<PagedResult<Resource>> GetAllResourcesAsync(ResourceQueryParams query);

        Task<Resource?> GetResourceByIdAsync(int id);

        Task CreateResourceAsync(Resource resource);

        Task UpdateResourceAsync(int id, Resource updatedResource);
        Task DeleteResourceAsync(int id);
    }
}