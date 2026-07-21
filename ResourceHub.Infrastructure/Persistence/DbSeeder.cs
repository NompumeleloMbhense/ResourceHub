using ResourceHub.Core.Entities;

namespace ResourceHub.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any(u => u.Role == "Admin"))
                return;

            context.Users.Add(new User
            {
                Username = "admin",
                Email = "admin@resourcehub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            });

            context.SaveChanges();
        }
    }
}