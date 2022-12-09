using System.Net;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Extensions;

public static class ProductExtensions
{
    public static Result<ProductEntity> IsAvailable(this ProductEntity productEntityDb)
        => productEntityDb != null && productEntityDb.AvailableQuantity.GetValueOrDefault() > 0
            ? Result.Ok(productEntityDb)
            : Result.Error<ProductEntity>(HttpStatusCode.BadRequest, "[NotAvailable]", ErrorMessages.NotAvailable);

    public static Result<ProductEntity> Validate(this ProductEntity productEntityDb)
        => productEntityDb != null
            ? Result.Ok(productEntityDb)
            : Result.Error<ProductEntity>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

    public static Result<ProductEntity> Hidrate(this ProductEntity dbEntity, ProductEntity entity)
    {
        dbEntity.Name = entity.Name;
        dbEntity.AvailableQuantity = entity.AvailableQuantity;
        dbEntity.Price = entity.Price;
        dbEntity.CategoryId = entity.CategoryId;
        return Result.Ok(dbEntity);
    }

    public static Result<ProductEntity> UpdateQuantity(this ProductEntity productEntity,
        double oldCartItemQuantity,
        double newCartItemQuantity)
    {
        if(oldCartItemQuantity < newCartItemQuantity)
            productEntity.AvailableQuantity -= (decimal?) (newCartItemQuantity - oldCartItemQuantity);
        else
            productEntity.AvailableQuantity += (decimal?) (oldCartItemQuantity - newCartItemQuantity);

        return Result.Ok(productEntity);
    }
}