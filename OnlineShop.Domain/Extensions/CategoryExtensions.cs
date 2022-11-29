using System.Net;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Domain.Extensions
{
    public static class CategoryExtensions
    {
        public static Result<secondaryPorts.Category> Validate(this secondaryPorts.Category product)
            => product != null
                   ? Result.Ok(product)
                   : Result.Error<secondaryPorts.Category>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        public static Result<secondaryPorts.Category> Hidrate(this secondaryPorts.Category dbEntity, secondaryPorts.Category entity)
        {
            dbEntity.Name = entity.Name;
            return Result.Ok(dbEntity);
        }
    }
}