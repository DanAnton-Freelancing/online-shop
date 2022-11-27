using System;
using System.Collections.Generic;
using System.Linq;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class ProductMapper
    {
        private static readonly Random Random = new Random();

        public static List<pp.Product> MapToPrimary(this List<sp.Product> products)
            => products.Select(l => l.MapToPrimary())
                       .ToList();

        public static pp.Product MapToPrimary(this sp.Product product)
            => new pp.Product
               {
                   Id = product.Id.GetValueOrDefault(),
                   Name = product.Name,
                   Code = product.Code,
                   Price = product.Price.GetValueOrDefault(),
                   AvailableQuantity = product.AvailableQuantity.GetValueOrDefault(),
                   IsAvailable = product.AvailableQuantity > 0,
                   CategoryId = product.CategoryId
               };

        public static List<sp.Product> MapToSecondary(this IList<pp.UpsertProduct> products)
            => products.Select(r => r.MapToUpsertSecondary())
                       .ToList();

        public static sp.Product MapToSecondary(this pp.UpsertProduct upsertProduct)
            => new sp.Product
               {
                   Name = upsertProduct.Name,
                   Price = upsertProduct.Price,
                   AvailableQuantity = upsertProduct.AvailableQuantity,
                   CategoryId = upsertProduct.CategoryId
               };

        private static sp.Product MapToUpsertSecondary(this pp.UpsertProduct upsertProduct)
            => new sp.Product
               {
                   Name = upsertProduct.Name,
                   Price = upsertProduct.Price,
                   AvailableQuantity = upsertProduct.AvailableQuantity,
                   CategoryId = upsertProduct.CategoryId,
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