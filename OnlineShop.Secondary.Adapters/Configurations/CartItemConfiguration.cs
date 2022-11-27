using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> entityBuilder)
        {
            entityBuilder.ToTable(nameof(CartItem));

            entityBuilder.Property(p => p.Price)
                         .HasColumnType("decimal(18,4)");

            entityBuilder.Property(p => p.Quantity)
                         .HasColumnType("decimal(18,2)");

            entityBuilder.HasOne(p => p.UserCart)
                         .WithMany(p => p.CartItems)
                         .HasForeignKey(p => p.UserCartId);

            entityBuilder.AddDefaultConfigsForDbEntity();
        }
    }
}