using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SaleAPI.Models;
using System.Collections.Generic;

namespace SaleAPI.DataAccess
{
    public class SaleAPIDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SalesPoint> SalesPoints { get; set; }

        public SaleAPIDataContext(DbContextOptions<SaleAPIDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buyer>().Property(b => b.SalesIds)
                .HasConversion(
                s => (string)JsonConvert.SerializeObject(s),
                s => JsonConvert.DeserializeObject<List<int>>(s));

            modelBuilder.Entity<Sale>().Property(s => s.SalesData)
                .HasConversion(
                s => (string)JsonConvert.SerializeObject(s),
                s => JsonConvert.DeserializeObject<List<SalesData>>(s));

            modelBuilder.Entity<SalesPoint>().Property(p => p.ProvidedProducts)
                .HasConversion(
                s => (string)JsonConvert.SerializeObject(s),
                s => JsonConvert.DeserializeObject<List<ProvidedProduct>>(s));

            modelBuilder.Entity<Sale>().Property(s => s.BuyerId).IsRequired(false);
        }        
    }
}
