using System.Collections.Generic;
using System.Linq;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class UserMapper
    {
        public static sp.User MapToSecondary(this pp.RegisterUser user)
            => new sp.User
               {
                   Email = user.Email,
                   Username = user.Username,
                   Password = user.Password,
                   FirstName = user.FirstName,
                   LastName = user.LastName
               };

        public static sp.CartItem MapToSecondary(this pp.UpsertCartItem upsertCartItem)
            => new sp.CartItem
               {
                   ProductId = upsertCartItem.ProductId,
                   Quantity = upsertCartItem.Quantity
               };

        public static pp.UserCart MapToPrimary(this sp.UserCart userCart)
            => new pp.UserCart
               {
                   Id = userCart.Id,
                   Total = userCart.Total,
                   CartItems = userCart.CartItems?.MapToPrimary()
               };

        private static List<pp.CartItem> MapToPrimary(this IEnumerable<sp.CartItem> cartItems)
            => cartItems.Select(c => c.MapToPrimary()).ToList();

        private static pp.CartItem MapToPrimary(this sp.CartItem cartItem)
            => new pp.CartItem
               {
                   Id = cartItem.Id,
                   ProductId = cartItem.ProductId,
                   Quantity = cartItem.Quantity,
                   Price = cartItem.Price,
                   ProductName = cartItem.Product?.Name
               };
    }
}