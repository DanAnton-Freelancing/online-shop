using System;

namespace OnlineShop.Secondary.Ports.DataContracts
{
    public class CartItem : EditableEntity
    {
        public double Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid UserCartId { get; set; }
        public Guid ProductId { get; set; }

        public virtual UserCart UserCart { get; set; }
        public virtual Product Product { get; set; }
    }
}