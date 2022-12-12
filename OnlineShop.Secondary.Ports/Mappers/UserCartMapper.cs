using OnlineShop.Domain.Aggregate;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class UserCartMapper
{
    public static UserCartEntity MapToDomain(this UserCart userCart) 
        => new()
        {
            Id = userCart.Id,
            UserId = userCart.UserId,
            CartItems = userCart.CartItems?.MapToDomain(),
            Version = userCart.Version
        };

    public static UserCart MapToPorts(this UserCartEntity userCartEntity)
        => new()
        {
            Id = userCartEntity.Id,
            UserId = userCartEntity.UserId,
            CartItems = userCartEntity.CartItems?.MapToPorts(),
            Version = userCartEntity.Version
        };
}