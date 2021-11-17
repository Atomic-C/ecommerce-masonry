using Masonry_Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Masonry_Data_Access
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

        public DbSet<InquiryHeader> InquiryHeaders { get; set; } // This property will be pushed to database
        public DbSet<InquiryDetails> InquiryDetails { get; set; } // This property will be pushed to database
        public DbSet<OrderHeader> OrderHeader { get; set; } // This property will be pushed to database
        public DbSet<OrderDetail> OrderDetail { get; set; } // This property will be pushed to database
    }
    // Whev we create a new model, this is what we do before migrations, we add hem as properties here.
}
