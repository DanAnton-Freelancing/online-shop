using System.Collections.Generic;
using System.Net;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Domain.Entities;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Extensions;

public static class UserCartExtensions
{
    public static Result Hidrate(this CartItemEntity cartItemEntityDb, ProductEntity p)
    {
        cartItemEntityDb.Price = p.Price.GetValueOrDefault() * (decimal) cartItemEntityDb.Quantity;
        return Result.Ok();
    }

    public static Result<UserEntity> AddUserCart(this UserEntity userEntityDb)
    {
        userEntityDb.UserCartEntity = new UserCartEntity();
        return Result.Ok(userEntityDb);
    }

    public static Result<UserCartEntity> AddCartItem(this UserCartEntity userCartEntity, CartItemEntity itemEntity, ProductEntity productEntity)
    {
        var cartItems = userCartEntity.CartItems;

        if (cartItems != null) {
            var existingProduct = cartItems.Find(ci => ci.ProductId.Equals(itemEntity.ProductId));

            if (existingProduct != null) {
                existingProduct.Quantity += itemEntity.Quantity;
                existingProduct.Price += itemEntity.Price;
            }
            else {
                userCartEntity.CartItems!.Add(itemEntity);
            }
        }
        else {
            userCartEntity.CartItems = new List<CartItemEntity> {itemEntity};
        }

        productEntity.UpdateQuantity(0, itemEntity.Quantity);
        itemEntity.ProductEntity = productEntity;

        return Result.Ok(userCartEntity);
    }

    public static Result<CartItemEntity> UpdateCartItem(this CartItemEntity cartItemEntity, double quantity)
    {
        var oldCartQuantity = cartItemEntity.Quantity;
        if (cartItemEntity.Quantity.Equals(quantity))
            return Result.Error<CartItemEntity>(HttpStatusCode.NotModified, "[NotChanged]", ErrorMessages.NotChanged);

        if (quantity > (double?) cartItemEntity.ProductEntity.AvailableQuantity + cartItemEntity.Quantity)
            return Result.Error<CartItemEntity>(HttpStatusCode.BadRequest, "[QuantityExceeded]",
                string.Format(ErrorMessages.QuantityExceeded,
                    cartItemEntity.ProductEntity.AvailableQuantity));
        cartItemEntity.Quantity = quantity;
        cartItemEntity.Price = (decimal) quantity * cartItemEntity.ProductEntity.Price.GetValueOrDefault();
        cartItemEntity.ProductEntity.UpdateQuantity(oldCartQuantity, cartItemEntity.Quantity);
        return Result.Ok(cartItemEntity);
    }
}