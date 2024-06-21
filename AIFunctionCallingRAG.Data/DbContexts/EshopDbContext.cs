using AIFunctionCallingRAG.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AIFunctionCallingRAG.Data.DbContexts
{
    public class EshopDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
