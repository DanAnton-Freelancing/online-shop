using System.Net;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Domain.Extensions
{
    public static class ProductExtensions
    {
        public static Result<secondaryPorts.Product> IsAvailable(this secondaryPorts.Product product)
            => product != null && product.AvailableQuantity.GetValueOrDefault() > 0
                   ? Result.Ok(product)
                   : Result.Error<secondaryPorts.Product>(HttpStatusCode.BadRequest, "[NotAvailable]", ErrorMessages.NotAvailable);

        public static Result<secondaryPorts.Product> Validate(this secondaryPorts.Product product)
            => product != null
                   ? Result.Ok(product)
                   : Result.Error<secondaryPorts.Product>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        public static Result<secondaryPorts.Product> Hidrate(this secondaryPorts.Product dbEntity, secondaryPorts.Product entity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.AvailableQuantity = entity.AvailableQuantity;
            dbEntity.Price = entity.Price;
            dbEntity.CategoryId = entity.CategoryId;
            return Result.Ok(dbEntity);
        }

        public static Result<secondaryPorts.Product> UpdateQuantity(this secondaryPorts.Product product,
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