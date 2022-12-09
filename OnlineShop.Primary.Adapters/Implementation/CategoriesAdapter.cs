using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Application.Implementations.Commands.Categories;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Primary.Adapters.Implementation;

public class CategoriesAdapter : CrudAdapter<CategoryModel, UpsertCategoryModel>, ICategoriesAdapter
{
    public CategoriesAdapter(IMediator mediator)
        => Mediator = mediator;

    public override async Task<Result<List<CategoryModel>>> GetAllAsync(CancellationToken cancellationToken)
        => await Mediator.Send(new GetCategoriesQuery(), cancellationToken)
            .MapAsync(c => c.MapToPrimary());

    public override async Task<Result<List<Guid>>> InsertAsync(List<UpsertCategoryModel> entities, CancellationToken cancellationToken)
        => await Mediator.Send(new AddCategoriesCommand { Data = entities.MapToSecondary() }, cancellationToken);

    public override async Task<Result<Guid>> UpdateAsync(Guid id, UpsertCategoryModel entity, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateCategoryCommand { Data = entity.MapToSecondary(), Id = id }, cancellationToken);

    public override async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteCategoryCommand {Id = id}, cancellationToken);
}