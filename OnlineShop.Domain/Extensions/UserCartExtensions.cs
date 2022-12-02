using System.Collections.Generic;
using System.Net;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Domain.Extensions;

public static class UserCartExtensions
{
    public static Result Hidrate(this CartItem cartItem, Product p)
    {
        cartItem.Price = p.Price.GetValueOrDefault() * (decimal) cartItem.Quantity;
        return Result.Ok();
    }

    public static Result<User> AddUserCart(this User user)
    {
        user.UserCart = new UserCart();
        return Result.Ok(user);
    }

    public static Result<UserCart> AddCartItem(this UserCart userCart, CartItem item)
    {
        var cartItems = userCart.CartItems;

        if (cartItems != null) {
            var existingProduct = cartItems.Find(ci => ci.ProductId.Equals(item.ProductId));

            if (existingProduct != null) {
                existingProduct.Quantity += item.Quantity;
                existingProduct.Price += item.Price;
            }
            else {
                item.UserCartId = userCart.Id.GetValueOrDefault();
                userCart.CartItems.Add(item);
            }
        }
        else {
            userCart.CartItems = new List<CartItem> {item};
        }

        return Result.Ok(userCart);
    }

    public static Result<CartItem> UpdateCartItem(this CartItem cartItem, double quantity)
    {
        if (cartItem.Quantity.Equals(quantity))
            return Result.Error<CartItem>(HttpStatusCode.NotModified, "[NotChanged]", ErrorMessages.NotChanged);

        if (quantity > (double?) cartItem.Product.AvailableQuantity + cartItem.Quantity)
            return Result.Error<CartItem>(HttpStatusCode.BadRequest, "[QuantityExceeded]",
                string.Format(ErrorMessages.QuantityExceeded,
                    cartItem.Product.AvailableQuantity));
        cartItem.Quantity = quantity;
        cartItem.Price = (decimal) quantity * cartItem.Product.Price.GetValueOrDefault();
        return Result.Ok(cartItem);
    }
}