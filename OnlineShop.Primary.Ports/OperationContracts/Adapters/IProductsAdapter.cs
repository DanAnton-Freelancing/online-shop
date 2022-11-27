using OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters
{
    public interface IProductsAdapter : ICrudAdapter<Product, UpsertProduct> { }
}