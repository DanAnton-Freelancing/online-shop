using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Secondary.Ports.DataContracts;

public class UserCart : EditableEntity
{
    public List<CartItem> CartItems { get; set; }
    public decimal Total => CartItems.Sum(ci => ci.Price);

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}