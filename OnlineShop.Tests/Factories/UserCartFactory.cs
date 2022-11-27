using System;
using System.Linq;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories
{
    public static class UserCartFactory
    {
        public static sp.CartItem CreateCartItem(sp.UserCart userCart)
        {
            var product = ProductFactory.Create().FirstOrDefault().ToEntity();
            var cartItem = new sp.CartItem
                           {
                               Price = 20,
                               Quantity = 1,
                               UserCart = userCart,
                               UserCartId = userCart?.Id ?? Guid.Empty,
                               Product = product,
                               ProductId = product.Id.GetValueOrDefault()
                           };
            return cartItem;
        }

        public static pp.UpsertCartItem CreateUpsertCartItem()
        {
            var product = ProductFactory.Create().FirstOrDefault().ToEntity();
            var cartItem = new pp.UpsertCartItem
                           {
                               Quantity = 1,
                               ProductId = product.Id.GetValueOrDefault()
                           };
            return cartItem;
        }
    }
}