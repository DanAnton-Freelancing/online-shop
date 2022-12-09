using OnlineShop.Domain.Entities;
using OnlineShop.Shared.Ports.DataContracts;


namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Users;

public interface IRegisterCommand : ICreateCommand<UserEntity, Result>
{
}