using System;
using System.Collections.Generic;
using System.Linq;
using OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers;

public static class ProductMapper
{
    private static readonly Random Random = new();

    public static List<primaryPorts.ProductModel> MapToPrimary(this List<Product> products)
        => products.Select(l => l.MapToPrimary())
            .ToList();

    public static primaryPorts.ProductModel MapToPrimary(this Product productDb)
        => new()
        {
            Id = productDb.Id.GetValueOrDefault(),
            Name = productDb.Name,
            Code = productDb.Code,
            Price = productDb.Price.GetValueOrDefault(),
            AvailableQuantity = productDb.AvailableQuantity.GetValueOrDefault(),
            IsAvailable = productDb.AvailableQuantity > 0,
            CategoryId = productDb.CategoryId,
            Description = productDb.Description,
            ImagesIds = productDb.Images?.Select(s => s.Id.GetValueOrDefault()).ToList()
        };

    public static List<Product> MapToSecondary(this IList<primaryPorts.UpsertProductModel> products)
        => products.Select(r => r.MapToUpsertSecondary())
            .ToList();

    public static Product MapToSecondary(this primaryPorts.UpsertProductModel upsertProductModel, IList<string> imagesKeys)
        => new()
        {
            Name = upsertProductModel.Name,
            Price = upsertProductModel.Price,
            AvailableQuantity = upsertProductModel.AvailableQuantity,
            CategoryId = upsertProductModel.CategoryId,
            Description = upsertProductModel.Description,
            Images = imagesKeys.Select(i => new Image { Key = i }).ToList(),
            Code = RandomString()

        };
    public static Product MapToSecondary(this primaryPorts.UpsertProductModel upsertProductModel)
        => new()
        {
            Name = upsertProductModel.Name,
            Price = upsertProductModel.Price,
            AvailableQuantity = upsertProductModel.AvailableQuantity,
            CategoryId = upsertProductModel.CategoryId,
            Code = RandomString()
        };

    private static Product MapToUpsertSecondary(this primaryPorts.UpsertProductModel upsertProductModel)
        => new()
        {
            Name = upsertProductModel.Name,
            Price = upsertProductModel.Price,
            AvailableQuantity = upsertProductModel.AvailableQuantity,
            CategoryId = upsertProductModel.CategoryId,
            Description = upsertProductModel.Description,
            Code = RandomString()
        };


    private static string RandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return "OS" + new string(Enumerable.Repeat(chars, 50)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}