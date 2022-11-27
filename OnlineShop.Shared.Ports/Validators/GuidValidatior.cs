using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Shared.Ports.Validators
{
    public static class GuidValidator
    {
        public static Result Validate(Guid id)
            => id.Equals(Guid.Empty)
                   ? Result.Error(HttpStatusCode.BadRequest, "[InvalidId]")
                   : Result.Ok();

        public static async Task<Result> ValidateAsync(Guid id, CancellationToken cancellationToken)
            => id.Equals(Guid.Empty)
                ? await Task.FromResult(Result.Error(HttpStatusCode.BadRequest, "[InvalidId]"))
                : await Task.FromResult(Result.Ok());
    }
}