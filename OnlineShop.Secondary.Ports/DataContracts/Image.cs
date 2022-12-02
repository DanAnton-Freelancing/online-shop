using System;

namespace OnlineShop.Secondary.Ports.DataContracts;

public class Image : EditableEntity
{
    public string Key { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}