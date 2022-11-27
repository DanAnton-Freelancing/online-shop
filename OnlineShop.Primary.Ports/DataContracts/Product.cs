using System;

namespace OnlineShop.Primary.Ports.DataContracts
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
    }
}