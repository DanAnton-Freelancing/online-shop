using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IUserCartReaderRepository : IBaseReaderRepository<UserCart>, IUserCartRepository
    {
    }
}