using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Commands.Products;

public class AddProductsCommand : IAddProductsCommand
{
    public Product Data { get; set; }
        
    public class AddProductsCommandHandler : IRequestHandler<AddProductsCommand, Result<Guid>>
    {
        private readonly IWriterRepository _writerRepository;

        public AddProductsCommandHandler(IWriterRepository writerRepository)
        {
            // add guard for repository 
            _writerRepository = writerRepository;
        }

        public async Task<Result<Guid>> Handle(AddProductsCommand request, CancellationToken cancellationToken) 
            => await _writerRepository.SaveAsync(request.Data, cancellationToken);
    }
}