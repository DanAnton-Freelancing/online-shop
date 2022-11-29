using System.Collections.Generic;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers
{
    public static class UserMapper
    {
        public static secondaryPorts.User MapToSecondary(this primaryPorts.RegisterUser user)
            => new()
            {
                   Email = user.Email,
                   Username = user.Username,
                   Password = user.Password,
                   FirstName = user.FirstName,
                   LastName = user.LastName
               };

        public static secondaryPorts.CartItem MapToSecondary(this primaryPorts.UpsertCartItem upsertCartItem)
            => new()
            {
                   ProductId = upsertCartItem.ProductId,
                   Quantity = upsertCartItem.Quantity
               };

        public static primaryPorts.UserCart MapToPrimary(this secondaryPorts.UserCart userCart)
            => new()
            {
                   Id = userCart.Id,
                   Total = userCart.Total,
                   CartItems = userCart.CartItems?.MapToPrimary()
               };

        private static List<primaryPorts.CartItem> MapToPrimary(this IEnumerable<secondaryPorts.CartItem> cartItems)
            => cartItems.Select(c => c.MapToPrimary()).ToList();

        private static primaryPorts.CartItem MapToPrimary(this secondaryPorts.CartItem cartItem)
            => new()
            {
                   Id = cartItem.Id,
                   ProductId = cartItem.ProductId,
                   Quantity = cartItem.Quantity,
                   Price = cartItem.Price,
                   ProductName = cartItem.Product?.Name
               };
    }
}