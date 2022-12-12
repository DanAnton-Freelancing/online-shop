using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class ProductMapper
{
    public static List<ProductEntity> MapToDomain(this List<Product> products)
        => products.Select(l => l.MapToDomain())
            .ToList();

    public static ProductEntity MapToDomain(this Product product)
        => new()
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Price = product.Price,
            AvailableQuantity = product.AvailableQuantity,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Images = product.Images?.MapToDomain(),
            Version = product.Version
        };

    public static List<Product> MapToPorts(this List<ProductEntity> products)
        => products.Select(l => l.MapToPorts())
            .ToList();

    public static Product MapToPorts(this ProductEntity productEntity)
        => new()
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Code = productEntity.Code,
            Price = productEntity.Price,
            AvailableQuantity = productEntity.AvailableQuantity,
            CategoryId = productEntity.CategoryId,
            Description = productEntity.Description,
            CartItem = productEntity.CartItemEntity?.MapToPorts(),
            Category = productEntity.CategoryEntity?.MapToPorts(),
            Images = productEntity.Images?.MapToPorts(),
            Version = productEntity.Version

        };
}