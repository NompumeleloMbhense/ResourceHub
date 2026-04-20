using ResourceHub.Shared.DTOs;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Client.Services
{
    public interface IResourceService
    {
        Task<PagedResult<ResourceDto>?> GetResourcesAsync(ResourceQueryParams query);
        Task<ResourceDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateResourceDto dto);
        Task<bool> UpdateAsync(int id, UpdateResourceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}