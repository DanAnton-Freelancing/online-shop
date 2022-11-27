using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Adapters.Configurations;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options)
            : base(options) => Database.Migrate();

        public DbSet<Product> Products { get; set; }
        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new UserCartConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}