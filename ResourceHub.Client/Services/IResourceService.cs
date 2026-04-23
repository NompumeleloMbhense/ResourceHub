using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public interface IResourceService
    {
        Task<PagedResult<ResourceDto>?> GetResourcesAsync(ResourceQueryParams query);
        Task<ResourceDto?> GetResourceByIdAsync(int id);
        Task<bool> CreateResourceAsync(CreateResourceDto dto);
        Task<bool> UpdateResourceAsync(int id, UpdateResourceDto dto);
        Task<bool> DeleteResourceAsync(int id);
    }
}