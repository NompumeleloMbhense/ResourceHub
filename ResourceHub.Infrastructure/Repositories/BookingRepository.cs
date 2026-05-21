using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;
using ResourceHub.Core.Interfaces;
using ResourceHub.Core.Pagination;
using ResourceHub.Core.QueryParams;
using ResourceHub.Infrastructure.Persistence;

namespace ResourceHub.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Booking>> GetAllAsync(BookingQueryParams query)
        {
            var bookingsQuery = _context.Bookings
                .AsNoTracking()
                .Include(b => b.Resource)
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();

                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookedBy.ToLower().Contains(search) ||
                    b.Purpose.ToLower().Contains(search) ||
                    b.Resource.Name.ToLower().Contains(search)
                );
            }

            // FILTERS
            if (query.ResourceId.HasValue)
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.ResourceId == query.ResourceId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.BookedBy))
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.BookedBy.Contains(query.BookedBy));
            }

            if (query.StartDate.HasValue)
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.StartTime >= query.StartDate.Value);
            }

            if (query.EndDate.HasValue)
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.EndTime <= query.EndDate.Value);
            }

            var totalCount = await bookingsQuery.CountAsync();

            var bookings = await bookingsQuery
                .OrderBy(b => b.StartTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Booking>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)query.PageSize
                ),
                Data = bookings
            };
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<PagedResult<Booking>> GetByResourceAsync(int resourceId, BookingQueryParams query)
        {
            var bookingsQuery = _context.Bookings
                .AsNoTracking()
                .Include(b => b.Resource)
                .Where(b => b.ResourceId == resourceId)
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();

                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookedBy.ToLower().Contains(search) ||
                    b.Purpose.ToLower().Contains(search) ||
                    b.Resource.Name.ToLower().Contains(search)
                );
            }

            // FILTERS
            if (!string.IsNullOrWhiteSpace(query.BookedBy))
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.BookedBy.Contains(query.BookedBy));
            }

            if (query.StartDate.HasValue)
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.StartTime >= query.StartDate.Value);
            }

            if (query.EndDate.HasValue)
            {
                bookingsQuery = bookingsQuery
                    .Where(b => b.EndTime <= query.EndDate.Value);
            }

            var totalCount = await bookingsQuery.CountAsync();

            var bookings = await bookingsQuery
                .OrderBy(b => b.StartTime)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Booking>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)query.PageSize
                ),
                Data = bookings
            };
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public void Delete(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}