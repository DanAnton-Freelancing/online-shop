using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Queries;

public class LoginQuery : ILoginQuery
{
    public string Username { get; set; }
    public string Password { get; set; }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<string>>
    {
        private readonly IReaderRepository _readerRepository;
        private readonly string _secret;

        public LoginQueryHandler(IReaderRepository readerRepository, string secret)
        {
            _secret = secret;
            _readerRepository = readerRepository;

        }

        public async Task<Result<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
            => await _readerRepository.GetOneAsync<User>(u => u.Username == request.Username, cancellationToken)
                                      .AndAsync(u => u.CheckPasswordHash(request.Password))
                                      .AndAsync(u => u.GenerateToken(_secret));
    }
}