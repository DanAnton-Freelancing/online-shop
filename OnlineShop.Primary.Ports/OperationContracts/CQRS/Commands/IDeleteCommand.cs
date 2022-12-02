using System;
using MediatR;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands;

public interface IDeleteCommand : IRequest<Result>
{
    Guid Id { get; set; }
}