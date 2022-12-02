using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Api.Extensions;

public static class ResultExtensions
{
    public static ObjectResult ToActionResult<T>(this Result<T> result) => new(result) {StatusCode = (int) result.HttpStatusCode};

    public static ObjectResult ToActionResult(this Result result) => new(result) {StatusCode = (int) result.HttpStatusCode};

    public static Task<ObjectResult> ToAsyncActionResult<T>(this Task<Result<T>> task)
        => Task.FromResult(new ObjectResult(task.Result) {StatusCode = (int)task.Result.HttpStatusCode});

    public static Task<ObjectResult> ToAsyncActionResult(this Task<Result> task)
        => Task.FromResult(new ObjectResult(task.Result) {StatusCode = (int)task.Result.HttpStatusCode });
}