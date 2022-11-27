using System.Net;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using sp = OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Domain.Extensions
{
    public static class CategoryExtensions
    {
        public static Result<sp.Category> Validate(this sp.Category product)
            => product != null
                   ? Result.Ok(product)
                   : Result.Error<sp.Category>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        public static Result<sp.Category> Hidrate(this sp.Category dbEntity, sp.Category entity)
        {
            dbEntity.Name = entity.Name;
            return Result.Ok(dbEntity);
        }
    }
}