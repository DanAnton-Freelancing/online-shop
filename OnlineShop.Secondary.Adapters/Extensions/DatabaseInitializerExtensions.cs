using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Secondary.Adapters.Initializer;

namespace OnlineShop.Secondary.Adapters.Extensions
{
    public static class DatabaseInitializerExtensions
    {
        public static void SeedInitialData(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope =
                applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
            DatabaseInitializer.Seed(context);
        }
    }
}