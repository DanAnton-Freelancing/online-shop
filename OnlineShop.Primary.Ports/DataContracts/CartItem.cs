using System;

namespace OnlineShop.Primary.Ports.DataContracts;

public class CartItem : BaseModel
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; }

    public double Quantity { get; set; }

    public decimal Price { get; set; }
}