using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Primary.Adapters
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPrimaryAdapters(this IServiceCollection services, string secret)
        {
            services.RegisterAllTypes(new[]
                                      {
                                          typeof(CrudAdapter<,>).Assembly
                                      });
            services.AddDomain(secret);
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}