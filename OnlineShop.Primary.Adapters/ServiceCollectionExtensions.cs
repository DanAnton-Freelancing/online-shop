using System.Reflection;
using Amazon.S3;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Primary.Adapters;

public static class ServiceCollectionExtensions
{
    public static void AddPrimaryAdapters(this IServiceCollection services, string secret, IConfiguration configuration)
    {
        services.RegisterAllTypes(new[]
        {
            typeof(CrudAdapter<,>).Assembly
        });
        services.AddDomain(secret);
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();
    }
}