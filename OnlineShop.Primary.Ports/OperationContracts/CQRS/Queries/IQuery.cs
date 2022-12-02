using MediatR;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IQuery<out T> : IRequest<T>
{
}