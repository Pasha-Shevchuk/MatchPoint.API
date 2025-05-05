using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchPoint.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // create roles for reader and writer
            var readerRoleId = "fed4bc99-d9b6-4151-a446-a18b3eaea903";
            var writerRoleId = "068b62db-ff04-4b68-b0c4-97467d3910f9";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER",
                    ConcurrencyStamp= writerRoleId
                }
            };

            // Seed the role
            builder.Entity<IdentityRole>().HasData(roles);

            // Create default user - ADMIN
            var adminUserId = "d07aec02-5470-4a03-9981-659d06ea808f";
            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@matchpoint.com",
                NormalizedUserName = "ADMIN@MATCHPOINT.COM",
                Email = "admin@matchpoint.com",
                NormalizedEmail = "ADMIN@MATCHPOINT.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAECOAJR9I9bPVylj7Tsgk9Gg3pJZmu+oQIk4pK244gIO2bv5rRohxFZtOpnZu50ndfg==",
                SecurityStamp = "STATIC_SECURITY_STAMP",
                ConcurrencyStamp = "STATIC_CONCURRENCY_STAMP",
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false
            };


            builder.Entity<IdentityUser>().HasData(admin);

            // give roles to admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }


    }
}
