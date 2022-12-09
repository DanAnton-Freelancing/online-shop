using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Entities;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers;

public static class CategoryMapper
{
    public static List<primaryPorts.CategoryModel> MapToPrimary(this List<CategoryEntity> categories)
        => categories.Select(l => l.MapToPrimary())
            .ToList();

    public static primaryPorts.CategoryModel MapToPrimary(this CategoryEntity categoryEntityDb)
        => new()
        {
            Id = categoryEntityDb.Id.GetValueOrDefault(),
            Name = categoryEntityDb.Name
        };

    public static List<CategoryEntity> MapToSecondary(this IList<primaryPorts.UpsertCategoryModel> categories)
        => categories.Select(r => r.MapToUpsertSecondary())
            .ToList();

    public static CategoryEntity MapToSecondary(this primaryPorts.UpsertCategoryModel upsertCategoryModel)
        => new()
        {
            Name = upsertCategoryModel.Name
        };

    private static CategoryEntity MapToUpsertSecondary(this primaryPorts.UpsertCategoryModel upsertCategoryModel)
        => new()
        {
            Name = upsertCategoryModel.Name
        };
}