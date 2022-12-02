using Microsoft.AspNetCore.Http;
using System;

namespace OnlineShop.Primary.Ports.DataContracts;

public class UpsertProduct : BaseUpsertModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal AvailableQuantity { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public IFormFile[] Files { get; set; }
}