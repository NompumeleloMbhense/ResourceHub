using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IResourceService
    {
        Task<List<Resource>> GetAllResourcesAsync();

        Task<Resource?> GetResourceByIdAsync(int id);

        Task<Resource> CreateResourceAsync(Resource resource);

        Task<bool> UpdateResourceAsync(int id, string name, string description, string location, int capacity, bool isAvailable);

        Task<bool> DeleteResourceAsync(int id);
    }
}