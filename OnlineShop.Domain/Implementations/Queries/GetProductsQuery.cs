using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Queries;

public class GetProductsQuery : IGetProductsQuery
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<Product>>>
    {
        private readonly IReaderRepository _readerRepository;

        public GetProductsQueryHandler(IReaderRepository readerRepository)
            => _readerRepository = readerRepository;

        public async Task<Result<List<Product>>> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
            => await _readerRepository.GetAsync<Product>(cancellationToken);
    }
}