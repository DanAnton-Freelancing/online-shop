using System;

namespace OnlineShop.Primary.Ports.DataContracts;

public class UpsertCartItem
{
    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public double Quantity { get; set; }
}