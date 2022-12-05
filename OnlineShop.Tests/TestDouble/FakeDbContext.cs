using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Adapters;

namespace OnlineShop.Tests.TestDouble
{
    public class FakeAppDbContext : DatabaseContext
    {
        public FakeAppDbContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new FakeEntityConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            base.SaveChangesAsync(cancellationToken);
            return Task.FromResult(1);
        }
    }
}