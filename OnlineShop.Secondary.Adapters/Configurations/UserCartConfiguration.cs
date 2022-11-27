using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations
{
    public class UserCartConfiguration : IEntityTypeConfiguration<UserCart>
    {
        public void Configure(EntityTypeBuilder<UserCart> entityBuilder)
        {
            entityBuilder.ToTable(nameof(UserCart));
            
            entityBuilder.AddDefaultConfigsForDbEntity();
        }
    }
}