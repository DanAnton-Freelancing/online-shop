using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> entityBuilder)
    {
        entityBuilder.ToTable(nameof(Image));
        entityBuilder.AddDefaultConfigsForDbEntity();
    }
}