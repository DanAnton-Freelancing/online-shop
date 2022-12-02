using System;
using System.Collections.Generic;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class ProductMapper
    {
        private static readonly Random Random = new();

        public static List<primaryPorts.Product> MapToPrimary(this List<secondaryPorts.Product> products)
            => products.Select(l => l.MapToPrimary())
                       .ToList();

        public static primaryPorts.Product MapToPrimary(this secondaryPorts.Product product)
            => new()
            {
                   Id = product.Id.GetValueOrDefault(),
                   Name = product.Name,
                   Code = product.Code,
                   Price = product.Price.GetValueOrDefault(),
                   AvailableQuantity = product.AvailableQuantity.GetValueOrDefault(),
                   IsAvailable = product.AvailableQuantity > 0,
                   CategoryId = product.CategoryId,
                   Description = product.Description
               };

        public static List<secondaryPorts.Product> MapToSecondary(this IList<primaryPorts.UpsertProduct> products)
            => products.Select(r => r.MapToUpsertSecondary())
                       .ToList();

        public static secondaryPorts.Product MapToSecondary(this primaryPorts.UpsertProduct upsertProduct, IList<string> imagesKeys)
            => new()
            {
                   Name = upsertProduct.Name,
                   Price = upsertProduct.Price,
                   AvailableQuantity = upsertProduct.AvailableQuantity,
                   CategoryId = upsertProduct.CategoryId,
                   Description = upsertProduct.Description,
                   Images = imagesKeys.Select(i => new secondaryPorts.Image { Key = i }).ToList()
               };
        public static secondaryPorts.Product MapToSecondary(this primaryPorts.UpsertProduct upsertProduct)
            => new()
            {
                Name = upsertProduct.Name,
                Price = upsertProduct.Price,
                AvailableQuantity = upsertProduct.AvailableQuantity,
                CategoryId = upsertProduct.CategoryId
            };

        private static secondaryPorts.Product MapToUpsertSecondary(this primaryPorts.UpsertProduct upsertProduct)
            => new()
            {
                   Name = upsertProduct.Name,
                   Price = upsertProduct.Price,
                   AvailableQuantity = upsertProduct.AvailableQuantity,
                   CategoryId = upsertProduct.CategoryId,
                   Description = upsertProduct.Description,
                   Code = RandomString()
               };


        private static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "OS" + new string(Enumerable.Repeat(chars, 50)
                                               .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}