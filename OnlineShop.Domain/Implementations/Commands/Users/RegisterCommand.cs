using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Users;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Domain.Implementations.Commands.Users;

public class RegisterCommand : IRegisterCommand
{
    public User Data { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;

        public RegisterCommandHandler(IWriterRepository writerRepository) 
            => _writerRepository = writerRepository;

        public Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
            => DoesUserNotExists(request,cancellationToken)
                                .AndAsync(request.Data.HashPassword)
                                .AndAsync(request.Data.AddUserCart)
                                .AndAsync(_ => _writerRepository.AddAsync(request.Data, cancellationToken))
                                .AndAsync(_ => _writerRepository.SaveAsync(request.Data, cancellationToken))
                                .RemoveDataAsync();

        private async Task<Result> DoesUserNotExists(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _writerRepository.GetOneAsync<User>(u => u.Username.Equals(request.Data.Username) ||
                                                                      u.Email.Equals(request.Data.Email),
                cancellationToken);
            return !user.HasErrors
                ? Result.Error<bool>(HttpStatusCode.Conflict, "[AlreadyExist]", ErrorMessages.UserAlreadyExist)
                : Result.Ok();
        }
    }
}