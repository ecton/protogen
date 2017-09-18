using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace CompoundKey.Models
{
    public class CompoundKeyDbContext : OverrideSuperclass
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreProduct>().HasKey(t => new { t.ProductId, t.StoreId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
