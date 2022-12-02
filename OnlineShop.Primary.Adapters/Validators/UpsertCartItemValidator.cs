using System;
using System.Net;
using System.Threading.Tasks;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Validators;

public static class UpsertCartItemValidator
{
    public static async Task<Result> ValidateAsync(UpsertCartItem item)
    {
        if (item == null)
            return await Task.FromResult(Result.Error(HttpStatusCode.BadRequest, "[InvalidInput]"));

        if (item.ProductId.Equals(Guid.Empty))
            return await Task.FromResult(Result.Error(HttpStatusCode.BadRequest, "[InvalidProductId]"));

        return item.Quantity <= 0
            ? await Task.FromResult(Result.Error(HttpStatusCode.BadRequest, "[InvalidQuantity]"))
            : await Task.FromResult(Result.Ok());
    }

    public static async Task<Result> ValidateQuantityAsync(double quantity)
        => quantity <= 0
            ? await Task.FromResult(Result.Error(HttpStatusCode.BadRequest, "[InvalidQuantity]"))
            : await Task.FromResult(Result.Ok());
}