using System.Collections.Generic;

namespace OnlineShop.Primary.Ports.DataContracts;

public class UserCartModel : BaseModel
{
    public List<CartItemModel> CartItems { get; set; }
    public decimal Total { get; set; }
}