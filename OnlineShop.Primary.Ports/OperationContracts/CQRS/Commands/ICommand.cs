using MediatR;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands;

public interface ICommand<out T>: IRequest<T>
{
}