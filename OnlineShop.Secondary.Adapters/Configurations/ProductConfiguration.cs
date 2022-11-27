using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.ToTable(nameof(Product));

            entityBuilder.Property(p => p.Price)
                         .HasColumnType("decimal(18,4)");

            entityBuilder.Property(p => p.AvailableQuantity)
                         .HasColumnType("decimal(18,2)");

            entityBuilder.HasOne(p => p.CartItem)
                         .WithOne(p => p.Product)
                         .HasForeignKey<CartItem>(p => p.ProductId);

            entityBuilder.HasOne(p => p.Category)
                         .WithMany(c => c.Products)
                         .IsRequired();
            entityBuilder.AddDefaultConfigsForDbEntity();
        }
    }
}