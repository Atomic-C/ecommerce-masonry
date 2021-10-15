using Masonry_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_masonry.Data
{
    public class ApplicationDbContext : IdentityDbContext // Inherit attributes and methods from DbContext class
    // DbContext only exists with sql server and entity framework packages installed

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; } // This property will be pushed to database, here we can set the name we want it to have within the DB
        public DbSet<ApplicationType> ApplicationType { get; set; } // This property will be pushed to database

        public DbSet<TransportCategory> TransportCategory { get; set; } // This property will be pushed to database

        public DbSet<Product> Product { get; set; } // This property will be pushed to database

        public DbSet<ApplicationUser> ApplicationUser { get; set; } // This property will be pushed to database
    }
}
