using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Entities;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class ImageMapper
{
    public static List<ImageEntity> MapToDomain(this IList<Image> images)
        => images.Select(l => l.MapToDomain())
            .ToList();

    public static ImageEntity MapToDomain(this Image imageDb)
        => new()
        {
            Id = imageDb.Id,
            ProductEntity = imageDb.Product?.MapToDomain(),
            ProductId = imageDb.ProductId,
            Key = imageDb.Key,
            Version = imageDb.Version

        };

    public static IList<Image> MapToPorts(this IList<ImageEntity> images)
        => images.Select(l => l.MapToPorts())
            .ToList();

    public static Image MapToPorts(this ImageEntity imageEntity)
        => new()
        {
            Id = imageEntity.Id,
            Product = imageEntity.ProductEntity?.MapToPorts(),
            ProductId = imageEntity.ProductId,
            Key = imageEntity.Key,
            Version = imageEntity.Version

        };
}