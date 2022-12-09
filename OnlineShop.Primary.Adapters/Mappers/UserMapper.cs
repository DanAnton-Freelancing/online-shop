using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Domain.Entities;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers;

public static class UserMapper
{
    public static UserEntity MapToSecondary(this primaryPorts.RegisterUserModel userModel)
        => new()
        {
            Email = userModel.Email,
            Username = userModel.Username,
            Password = userModel.Password,
            FirstName = userModel.FirstName,
            LastName = userModel.LastName
        };

    public static CartItemEntity MapToSecondary(this primaryPorts.UpsertCartItemModel upsertCartItemModel)
        => new()
        {
            ProductId = upsertCartItemModel.ProductId,
            Quantity = upsertCartItemModel.Quantity
        };

    public static primaryPorts.UserCartModel MapToPrimary(this UserCartEntity userCartEntityDb)
        => new()
        {
            Id = userCartEntityDb.Id,
            Total = userCartEntityDb.Total,
            CartItems = userCartEntityDb.CartItems?.MapToPrimary()
        };

    private static List<primaryPorts.CartItemModel> MapToPrimary(this IEnumerable<CartItemEntity> cartItems)
        => cartItems.Select(c => c.MapToPrimary()).ToList();

    private static primaryPorts.CartItemModel MapToPrimary(this CartItemEntity cartItemEntityDb)
        => new()
        {
            Id = cartItemEntityDb.Id,
            ProductId = cartItemEntityDb.ProductId,
            Quantity = cartItemEntityDb.Quantity,
            Price = cartItemEntityDb.Price,
            ProductName = cartItemEntityDb.ProductEntity?.Name
        };
}