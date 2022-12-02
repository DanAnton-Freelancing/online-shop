using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineShop.Shared.Ports.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterAllTypes(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        string requestHandler = "")
    {
        var typesFromAssemblies = assemblies.SelectMany(a => a.GetExportedTypes().Where(x => !x.IsAbstract));
        foreach (var type in typesFromAssemblies) {
            var typeInterface = type.GetInterfaces().FirstOrDefault(t => t.Name.Contains(type.Name) ||
                                                                         !requestHandler.Equals("") &&
                                                                         t.Name.Contains(requestHandler));
            if (typeInterface != null)
                services.Add(new ServiceDescriptor(typeInterface, type, lifetime));
        }
    }
}