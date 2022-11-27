using System;

namespace OnlineShop.Secondary.Ports.DataContracts
{
    public class Product : EditableEntity
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }

        public decimal? AvailableQuantity { get; set; }

        public string Code { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual CartItem CartItem { get; set; }
    }
}