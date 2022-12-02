using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Users;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Users;

public class RegisterCommand : IRegisterCommand
{
    public User Data { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IUserWriterRepository _userWriterRepository;

        public RegisterCommandHandler(IUserWriterRepository userWriterRepository)
        {
            _userWriterRepository = userWriterRepository;
        }

        public Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
            => _userWriterRepository.UserNotExistsAsync(request.Data, cancellationToken)
                .AndAsync(request.Data.HashPassword)
                .AndAsync(request.Data.AddUserCart)
                .AndAsync(u => _userWriterRepository.SaveAsync(request.Data, cancellationToken))
                .RemoveDataAsync();
    }
}