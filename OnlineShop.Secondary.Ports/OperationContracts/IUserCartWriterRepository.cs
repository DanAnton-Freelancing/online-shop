using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IUserCartWriterRepository : IBaseWriterRepository<UserCart>, IUserCartRepository { }