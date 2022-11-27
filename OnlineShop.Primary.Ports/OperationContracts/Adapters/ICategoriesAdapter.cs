using OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters
{
    public interface ICategoriesAdapter : ICrudAdapter<Category, UpsertCategory> { }
}