using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<T> AddDefaultConfigsForDbEntity<T>(this EntityTypeBuilder<T> builder)
        where T : Editable
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(p => p.Version)
            .IsRowVersion();

        return builder;
    }
}