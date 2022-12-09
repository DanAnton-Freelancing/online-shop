using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Aggregate;

public class UserCartEntity : EditableEntity
{
    public List<CartItemEntity>? CartItems { get; set; }
    public decimal Total => CartItems.Sum(ci => ci.Price);

    public Guid UserId { get; set; }
    public virtual UserEntity UserEntity { get; set; }
}