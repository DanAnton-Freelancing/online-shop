using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Products
{
    public class DeleteProductCommand : IDeleteProductCommand
    {
        public Guid Id { get; set; }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
        {
            private readonly IProductWriterRepository _productWriterRepository;
            public DeleteProductCommandHandler(IProductWriterRepository productWriterRepository)
                => _productWriterRepository = productWriterRepository;

            public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
                => await _productWriterRepository.GetAsync(request.Id, cancellationToken)
                                                 .AndAsync(p => _productWriterRepository.CheckIfIsUsedAsync(p.Id.GetValueOrDefault(),cancellationToken))
                                                 .AndAsync(() => _productWriterRepository.DeleteAsync(request.Id, cancellationToken))
                                                 .RemoveDataAsync();
        }
    }
}
