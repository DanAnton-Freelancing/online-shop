using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Aggregate;

public class CartItemEntity : EditableEntity
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }

    public Guid UserCartId { get; set; }
    public Guid ProductId { get; set; }

    public virtual UserCartEntity? UserCartEntity { get; set; }
    public virtual ProductEntity? ProductEntity { get; set; }
}