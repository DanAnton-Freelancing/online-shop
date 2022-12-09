using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Entities;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Commands.Categories;

public class AddCategoriesCommand: IAddCategoriesCommand
{
    public List<CategoryEntity> Data { get; set; }

    public class AddCategoriesCommandHandler : IRequestHandler<AddCategoriesCommand, Result<List<Guid>>>
    {
        private readonly IWriterRepository _writerRepository;
        public AddCategoriesCommandHandler(IWriterRepository writerRepository)
        {
            // add Guard in the future
            _writerRepository = writerRepository;
        }

        public async Task<Result<List<Guid>>> Handle(AddCategoriesCommand request,
            CancellationToken cancellationToken)
            => await _writerRepository.AddAsync(request.Data.MapToPorts(), cancellationToken)
                                      .AndAsync(c => _writerRepository.AddAsync(c, cancellationToken))
                                      .AndAsync(c => _writerRepository.SaveAsync(c, cancellationToken));
    }
}