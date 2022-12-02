using System;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands;

public interface IUpdateCommand<T, out TI>: ICommand<TI>
{
    Guid Id { get; set; }
    T Data { get; set; }
}