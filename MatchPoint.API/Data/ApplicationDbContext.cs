using MatchPoint.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatchPoint.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }



    }
}
