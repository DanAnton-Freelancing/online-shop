using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Queries;

public class GetCategoriesQuery : IGetCategoriesQuery
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<Category>>>
    {
        private readonly ICategoryReaderRepository _categoryReaderRepository;

        public GetCategoriesQueryHandler(ICategoryReaderRepository categoryReaderRepository)
            => _categoryReaderRepository = categoryReaderRepository;

        public async Task<Result<List<Category>>> Handle(GetCategoriesQuery request,
            CancellationToken cancellationToken)
            => await _categoryReaderRepository.GetAsync(cancellationToken);
    }
}