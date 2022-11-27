using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entityBuilder)
        {
            entityBuilder.ToTable(nameof(Category));
            entityBuilder.AddDefaultConfigsForDbEntity();
        }
    }
}