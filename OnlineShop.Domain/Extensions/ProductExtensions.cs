using System.Net;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using sp = OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Domain.Extensions
{
    public static class ProductExtensions
    {
        public static Result<sp.Product> IsAvailable(this sp.Product product)
            => product != null && product.AvailableQuantity.GetValueOrDefault() > 0
                   ? Result.Ok(product)
                   : Result.Error<sp.Product>(HttpStatusCode.BadRequest, "[NotAvailable]", ErrorMessages.NotAvailable);

        public static Result<sp.Product> Validate(this sp.Product product)
            => product != null
                   ? Result.Ok(product)
                   : Result.Error<sp.Product>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        public static Result<sp.Product> Hidrate(this sp.Product dbEntity, sp.Product entity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.AvailableQuantity = entity.AvailableQuantity;
            dbEntity.Price = entity.Price;
            dbEntity.CategoryId = entity.CategoryId;
            return Result.Ok(dbEntity);
        }

        public static Result<sp.Product> UpdateQuantity(this sp.Product product,
                                                        double oldCartItemQuantity,
                                                        double newCartItemQuantity)
        {
            if(oldCartItemQuantity < newCartItemQuantity)
                product.AvailableQuantity -= (decimal?) (newCartItemQuantity - oldCartItemQuantity);
            else
                product.AvailableQuantity += (decimal?) (oldCartItemQuantity - newCartItemQuantity);

            return Result.Ok(product);
        }
    }
}