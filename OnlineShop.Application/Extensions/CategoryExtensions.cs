using System.Net;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Extensions;

public static class CategoryExtensions
{
    public static Result<Category> Validate(this Category product)
        => product != null
            ? Result.Ok(product)
            : Result.Error<Category>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

    public static Result<Category> Hidrate(this Category dbEntity, Category entity)
    {
        dbEntity.Name = entity.Name;
        return Result.Ok(dbEntity);
    }
}