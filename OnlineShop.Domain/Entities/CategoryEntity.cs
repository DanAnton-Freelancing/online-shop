using OnlineShop.Domain.Aggregate;
using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Entities;

public class CategoryEntity : EditableEntity
{
    public string Name { get; set; }

    public virtual List<ProductEntity> Products { get; set; }
}