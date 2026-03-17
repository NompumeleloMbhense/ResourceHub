using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Entities;

namespace ResourceHub.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<Booking> bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships

            modelBuilder.Entity<Resource>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Resource)
                .HasForeignKey(b => b.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Purpose)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
