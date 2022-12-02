using System.Collections.Generic;

namespace OnlineShop.Primary.Ports.DataContracts;

public class UserCart : BaseModel
{
    public List<CartItem> CartItems { get; set; }
    public decimal Total { get; set; }
}