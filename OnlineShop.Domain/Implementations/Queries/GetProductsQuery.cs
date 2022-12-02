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
        private readonly IProductReaderRepository _productReaderRepository;

        public GetProductsQueryHandler(IProductReaderRepository productReaderRepository)
            => _productReaderRepository = productReaderRepository;

        public async Task<Result<List<Product>>> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
            => await _productReaderRepository.GetAsync(cancellationToken);
    }
}