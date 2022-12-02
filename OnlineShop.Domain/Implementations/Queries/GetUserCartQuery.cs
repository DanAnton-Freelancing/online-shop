using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Queries;

public class GetUserCartQuery : IGetUserCartQuery
{
    public Guid userId { get; set; }

    public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, Result<UserCart>>
    {
        private readonly IUserCartReaderRepository _userCartReaderRepository;

        public GetUserCartQueryHandler(IUserCartReaderRepository userCartReaderRepository)
        {
            _userCartReaderRepository = userCartReaderRepository;
        }

        public async Task<Result<UserCart>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
            => await _userCartReaderRepository.GetWithDetailsAsync(request.userId, cancellationToken);
    }
}