using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Entities;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class CategoryMapper
{
    public static List<CategoryEntity> MapToDomain(this List<Category> categories)
        => categories.Select(l => l.MapToDomain())
            .ToList();

    public static CategoryEntity MapToDomain(this Category categoryDb)
        => new()
        {
            Id = categoryDb.Id,
            Name = categoryDb.Name
        };

    public static List<Category> MapToPorts(this List<CategoryEntity> categories)
        => categories.Select(l => l.MapToPorts())
            .ToList();

    public static Category MapToPorts(this CategoryEntity categoryEntity)
        => new()
        {
            Id = categoryEntity.Id,
            Name = categoryEntity.Name
        };
}