using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomain(this IServiceCollection services, string secret)
    {
        services.RegisterAllTypes(
            new[]
            {
                typeof(ServiceCollectionExtensions).Assembly
            },
            ServiceLifetime.Scoped,
            "RequestHandler"
        );

        services.AddScoped<IRequestHandler<LoginQuery, Result<string>>>(
            s => new LoginQuery.LoginQueryHandler(s.GetRequiredService<IReaderRepository>(), secret));
    }
}