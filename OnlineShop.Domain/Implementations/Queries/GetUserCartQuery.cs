using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IReaderRepository _readerRepository;

        public GetUserCartQueryHandler(IReaderRepository readerRepository) 
            => _readerRepository = readerRepository;

        public async Task<Result<UserCart>> Handle(GetUserCartQuery request, CancellationToken cancellationToken) =>
            await _readerRepository.GetOneAsync<UserCart>(c => c.UserId == request.userId, 
                                                          cancellationToken, null,
                                                          c => c.Include(u => u.CartItems)
                                                                .ThenInclude(u => u.Product));
    }
}