using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Domain.Entities;
using UserManagement.Application.Security;
using UserManagement.Domain.Enums;

namespace UserManagement.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            if (!context.Users.Any(u => u.IsAdmin))
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Admin",
                    PasswordHash = PasswordHasher.Hash("Admin123"),
                    Name = "Administrator",
                    Gender = Gender.Unknown,
                    Birthday = null,
                    IsAdmin = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
