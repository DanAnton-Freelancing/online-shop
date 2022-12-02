using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IBaseReaderRepository<T> : IBaseRepository<T>
    where T : EditableEntity
{
}