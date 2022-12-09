using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Aggregate;

public class ProductEntity : EditableEntity
{
    public string Name { get; set; }

    public decimal? Price { get; set; }

    public decimal? AvailableQuantity { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public IList<ImageEntity>? Images { get; set; }

    public Guid CategoryId { get; set; }

    public virtual CategoryEntity? CategoryEntity { get; set; }

    public virtual CartItemEntity? CartItemEntity { get; set; }
}