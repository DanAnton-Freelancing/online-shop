using System.Collections.Generic;
using System.Linq;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class CartItemMapper
{
    public static List<CartItemEntity> MapToDomain(this List<CartItem> cartItems)
        => cartItems.Select(l => l.MapToDomain())
            .ToList();

    public static CartItemEntity MapToDomain(this CartItem cartItemDb)
        => new()
        {
            Id = cartItemDb.Id,
            Price = cartItemDb.Price,
            ProductEntity = cartItemDb.Product?.MapToDomain(),
            ProductId = cartItemDb.ProductId,
            Quantity = cartItemDb.Quantity,
            UserCartEntity = cartItemDb.UserCart?.MapToDomain(),
            Version = cartItemDb.Version
        };

    public static List<CartItem> MapToPorts(this List<CartItemEntity> cartItems)
        => cartItems.Select(l => l.MapToPorts())
            .ToList();

    public static CartItem MapToPorts(this CartItemEntity cartItemEntity)
        => new()
        {
            Id = cartItemEntity.Id,
            Price = cartItemEntity.Price,
            Product = cartItemEntity.ProductEntity?.MapToPorts(),
            ProductId = cartItemEntity.ProductId,
            Quantity = cartItemEntity.Quantity,
            UserCart = cartItemEntity.UserCartEntity?.MapToPorts(),
            Version = cartItemEntity.Version
        };
}