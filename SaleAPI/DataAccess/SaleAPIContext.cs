using Microsoft.EntityFrameworkCore;
using SaleAPI.Models;

namespace SaleAPI.DataAccess
{
    public class SaleAPIDataContext : DbContext
    {
        public SaleAPIDataContext(DbContextOptions<SaleAPIDataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SalesPoint> SalesPoints { get; set; }
    }
}
