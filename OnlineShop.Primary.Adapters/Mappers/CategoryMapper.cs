using System.Collections.Generic;
using System.Linq;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class CategoryMapper
    {
        public static List<pp.Category> MapToPrimary(this List<sp.Category> products)
            => products.Select(l => l.MapToPrimary())
                       .ToList();

        public static pp.Category MapToPrimary(this sp.Category product)
            => new pp.Category
               {
                   Id = product.Id.GetValueOrDefault(),
                   Name = product.Name
               };

        public static List<sp.Category> MapToSecondary(this IList<pp.UpsertCategory> products)
            => products.Select(r => r.MapToUpsertSecondary())
                       .ToList();

        public static sp.Category MapToSecondary(this pp.UpsertCategory upsertProduct)
            => new sp.Category
               {
                   Name = upsertProduct.Name
               };

        private static sp.Category MapToUpsertSecondary(this pp.UpsertCategory upsertProduct)
            => new sp.Category
               {
                   Name = upsertProduct.Name
               };
    }
}