using System.Collections.Generic;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers;

public static class CategoryMapper
{
    public static List<primaryPorts.Category> MapToPrimary(this List<secondaryPorts.Category> categories)
        => categories.Select(l => l.MapToPrimary())
            .ToList();

    public static primaryPorts.Category MapToPrimary(this secondaryPorts.Category category)
        => new()
        {
            Id = category.Id.GetValueOrDefault(),
            Name = category.Name
        };

    public static List<secondaryPorts.Category> MapToSecondary(this IList<primaryPorts.UpsertCategory> categories)
        => categories.Select(r => r.MapToUpsertSecondary())
            .ToList();

    public static secondaryPorts.Category MapToSecondary(this primaryPorts.UpsertCategory upsertCategory)
        => new()
        {
            Name = upsertCategory.Name
        };

    private static secondaryPorts.Category MapToUpsertSecondary(this primaryPorts.UpsertCategory upsertCategory)
        => new()
        {
            Name = upsertCategory.Name
        };
}