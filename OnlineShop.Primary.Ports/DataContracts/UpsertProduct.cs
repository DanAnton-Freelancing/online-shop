using System;

namespace OnlineShop.Primary.Ports.DataContracts
{
    public class UpsertProduct : BaseUpsertModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal AvailableQuantity { get; set; }
        public Guid CategoryId { get; set; }
    }
}