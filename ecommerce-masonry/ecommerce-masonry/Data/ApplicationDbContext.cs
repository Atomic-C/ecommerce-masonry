using ecommerce_masonry.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_masonry.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
    }
}
