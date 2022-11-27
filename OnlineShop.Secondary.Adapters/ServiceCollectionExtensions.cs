using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Secondary.Adapters.Implementation;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Secondary.Adapters
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSecondaryAdapters(this IServiceCollection services, string connectionString)
        {
            services.RegisterAllTypes(new[]
                                      {
                                          typeof(BaseRepository<>).Assembly,
                                          typeof(BaseWriterRepository<>).Assembly
                                      });

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
        }
    }
}