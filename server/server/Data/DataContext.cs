using Microsoft.EntityFrameworkCore;
using server.Models;


namespace server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {      
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SellerId);
        }
    }
}
