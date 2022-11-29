using System.Collections.Generic;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class CategoryMapper
    {
        public static List<primaryPorts.Category> MapToPrimary(this List<secondaryPorts.Category> products)
            => products.Select(l => l.MapToPrimary())
                       .ToList();

        public static primaryPorts.Category MapToPrimary(this secondaryPorts.Category product)
            => new()
            {
                   Id = product.Id.GetValueOrDefault(),
                   Name = product.Name
               };

        public static List<secondaryPorts.Category> MapToSecondary(this IList<primaryPorts.UpsertCategory> products)
            => products.Select(r => r.MapToUpsertSecondary())
                       .ToList();

        public static secondaryPorts.Category MapToSecondary(this primaryPorts.UpsertCategory upsertProduct)
            => new()
            {
                   Name = upsertProduct.Name
               };

        private static secondaryPorts.Category MapToUpsertSecondary(this primaryPorts.UpsertCategory upsertProduct)
            => new()
            {
                   Name = upsertProduct.Name
               };
    }
}