using ResourceHub.Core.Pagination;
using ResourceHub.Core.QueryParams;
using ResourceHub.Core.Entities;

namespace ResourceHub.Core.Interfaces
{
    public interface IBookingRepository
    {Task<PagedResult<Booking>> GetAllAsync(BookingQueryParams query);

        Task<Booking?> GetByIdAsync(int id);
        Task<PagedResult<Booking>> GetByResourceAsync(int resourceId, BookingQueryParams query);
        Task AddAsync(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
        Task SaveChangesAsync();
    }    
}