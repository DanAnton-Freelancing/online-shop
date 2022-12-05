// ReSharper disable VirtualMemberCallInConstructor
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Adapters.Configurations;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
        if (Database.IsRelational())
        {
            Database.Migrate();
        }
    }

    public DbSet<Category> Categories { get; set; }

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