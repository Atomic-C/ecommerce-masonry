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
        public DbSet<Category> Category { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
    }
}
