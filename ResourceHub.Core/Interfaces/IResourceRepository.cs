using ResourceHub.Core.Entities;
using ResourceHub.Shared.Pagination;
using ResourceHub.Shared.QueryParams;

namespace ResourceHub.Core.Interfaces
{
    public interface IResourceRepository
    {
        Task<PagedResult<Resource>> GetAllAsync(ResourceQueryParams query);

        Task<Resource?> GetByIdAsync(int id);

        Task AddAsync(Resource resource);

        void Update(Resource resource);

        void Delete(Resource resource);

        Task SaveChangesAsync();
    }
}