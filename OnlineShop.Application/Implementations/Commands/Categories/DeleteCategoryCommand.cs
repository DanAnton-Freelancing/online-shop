using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Implementations.Commands.Categories;

public class DeleteCategoryCommand : IDeleteCategoryCommand
{
    public Guid Id { get; set; }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;

        public DeleteCategoryCommandHandler(IWriterRepository writerRepository)
        {
            _writerRepository = writerRepository;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _writerRepository.GetOneAsync<Category>(c => c.Id == request.Id,
                    cancellationToken, null, c => c.Include(u => u.Products))
                .AndAsync(CheckIfIsUsedAsync)
                .AndAsync(_ => _writerRepository.DeleteAsync<Category>(request.Id, cancellationToken))
                .AndAsync(() => _writerRepository.SaveAsync(cancellationToken))
                .RemoveDataAsync();
        }

        private static Result<Category> CheckIfIsUsedAsync(Category categoryDb) 
            => categoryDb?.Products?.Count > 0
                ? Result.Error<Category>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted)
                : Result.Ok(categoryDb);
    }
}