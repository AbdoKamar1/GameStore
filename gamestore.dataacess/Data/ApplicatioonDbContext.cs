using gamestore.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace gamestore.dataacess.Data
{
    public class ApplicatioonDbContext :IdentityDbContext
    {
        public ApplicatioonDbContext(DbContextOptions<ApplicatioonDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails>  OrderDetails { get; set; }


    }
}