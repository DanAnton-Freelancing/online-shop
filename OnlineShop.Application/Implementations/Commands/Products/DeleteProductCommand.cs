using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Implementations.Commands.Products;

public class DeleteProductCommand : IDeleteProductCommand
{
    public Guid Id { get; set; }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;
        public DeleteProductCommandHandler(IWriterRepository writerRepository)
            => _writerRepository = writerRepository;

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            => await _writerRepository.GetOneAsync<Product>(p => p.Id == request.Id, cancellationToken, null,
                                                            p => p.Include(c => c.CartItem))
                                      .AndAsync(CheckIfIsUsedAsync)
                                      .AndAsync(_ => _writerRepository.DeleteAsync<Product>(request.Id, cancellationToken))
                                      .AndAsync(() => _writerRepository.SaveAsync(cancellationToken))
                                      .RemoveDataAsync();

        private static Result<Product> CheckIfIsUsedAsync(Product productDb) 
            => productDb?.CartItem != null 
                ? Result.Error<Product>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted)
                : Result.Ok(productDb);
    }
}