using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Entities;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Queries;

public class GetCategoriesQuery : IGetCategoriesQuery
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryEntity>>>
    {
        private readonly IReaderRepository _readerRepository;

        public GetCategoriesQueryHandler(IReaderRepository readerRepository)
            => _readerRepository = readerRepository;

        public async Task<Result<List<CategoryEntity>>> Handle(GetCategoriesQuery request,
            CancellationToken cancellationToken)
            => await _readerRepository.GetAsync<Category>(cancellationToken)
                .MapAsync(c=>c.MapToDomain());
    }
}