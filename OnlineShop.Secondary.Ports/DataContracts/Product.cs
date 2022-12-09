using System;
using System.Collections.Generic;

namespace OnlineShop.Secondary.Ports.DataContracts;

public class Product : Editable
{
    public string Name { get; set; }

    public decimal? Price { get; set; }

    public decimal? AvailableQuantity { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public IList<Image> Images { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual CartItem? CartItem { get; set; }
}