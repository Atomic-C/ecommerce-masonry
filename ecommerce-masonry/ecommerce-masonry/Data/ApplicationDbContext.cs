using ecommerce_masonry.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_masonry.Data
{
    public class ApplicationDbContext : DbContext // Inherit attributes and methods from DbContext class
    // DbContext only exists with sql server and entity framework packages installed

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; } // This property will be pushed to database, here we can set the name we want it to have within the DB
        public DbSet<ApplicationType> ApplicationType { get; set; } // This property will be pushed to database
    }
}
