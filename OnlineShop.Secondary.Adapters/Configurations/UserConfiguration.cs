using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Adapters.Extensions;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.ToTable(nameof(User));
        entityBuilder.Property(u => u.Salt)
            .IsRequired();

        entityBuilder.Property(u => u.Username)
            .IsRequired();

        entityBuilder.Property(u => u.Email)
            .IsRequired();

        entityBuilder.Property(u => u.Password)
            .IsRequired();

        entityBuilder.AddDefaultConfigsForDbEntity();
    }
}