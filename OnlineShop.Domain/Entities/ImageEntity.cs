using OnlineShop.Domain.Aggregate;
using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Entities;

public class ImageEntity : EditableEntity
{
    public string Key { get; set; }
    public Guid ProductId { get; set; }
    public virtual ProductEntity ProductEntity { get; set; }
}