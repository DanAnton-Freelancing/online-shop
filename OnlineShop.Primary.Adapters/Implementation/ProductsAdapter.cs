using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Implementations.Commands.Products;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Primary.Adapters.Implementation
{
    public class ProductsAdapter : CrudAdapter<Product, UpsertProduct>, IProductsAdapter
    {
        public ProductsAdapter(IMediator mediator)
            => Mediator = mediator;

        public override async Task<Result<List<Product>>> GetAllAsync(CancellationToken cancellationToken)
        => await Mediator.Send(new GetProductsQuery(), cancellationToken)
                         .MapAsync(p => p.MapToPrimary());

        public override async Task<Result<List<Guid>>> InsertAsync(List<UpsertProduct> entities, CancellationToken cancellationToken)
            => await Mediator.Send(new AddProductsCommand { Data = entities.MapToSecondary() }, cancellationToken);

        public override async Task<Result<Guid>> UpdateAsync(Guid id, UpsertProduct entity, CancellationToken cancellationToken)
            => await Mediator.Send(new UpdateProductCommand { Id = id, Data = entity.MapToSecondary() }, cancellationToken);

        public override async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
            => await Mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);

    }
}