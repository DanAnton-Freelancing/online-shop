using System;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories;

public static class UserCartFactory
{
    public static secondaryPorts.CartItemDb CreateCartItem(secondaryPorts.UserCartDb userCartDb)
    {
        var product = ProductFactory.Create().FirstOrDefault().ToEntity();
        var cartItem = new secondaryPorts.CartItemDb
        {
            Price = 20,
            Quantity = 1,
            UserCartDb = userCartDb,
            UserCartId = userCartDb?.Id ?? Guid.Empty,
            ProductDb = product,
            ProductId = product.Id.GetValueOrDefault()
        };
        return cartItem;
    }

    public static primaryPorts.UpsertCartItemModel CreateUpsertCartItem()
    {
        var product = ProductFactory.Create().FirstOrDefault().ToEntity();
        var cartItem = new primaryPorts.UpsertCartItemModel
        {
            Quantity = 1,
            ProductId = product.Id.GetValueOrDefault()
        };
        return cartItem;
    }
}