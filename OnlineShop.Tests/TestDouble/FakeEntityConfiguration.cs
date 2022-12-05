using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeEntityConfiguration : IEntityTypeConfiguration<FakeEntity>
    {
        public void Configure(EntityTypeBuilder<FakeEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(c => c.Id);
            builder.Property(c => c.Name);
            builder.HasOne(t => t.Child).WithMany(s => s.FakeEntities);
        }
    }
}