using System;
using System.Threading.Tasks;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Shared.Ports.Extensions;

public static class TaskResultExtensions
{
    public static async Task<Result> AndAsync(this Task<Result> current,
        Func<Result> otherFunc)
    {
        var currentResult = current.Result;
        if (currentResult.HasErrors)
            return await current;

        var r1 = otherFunc();
        currentResult.HttpStatusCode = r1.HttpStatusCode;
        currentResult.Messages.AddRange(r1.Messages);
        return await current;
    }

    public static async Task<Result> AndAsync(this Task<Result> current,
        Func<Task<Result>> otherFunc)
    {
        var currentResult = current.Result;
        if (currentResult.HasErrors)
            return await current;

        var r1 = await otherFunc();
        currentResult.HttpStatusCode = r1.HttpStatusCode;
        currentResult.Messages.AddRange(r1.Messages);
        return await current;
    }

    public static async Task<Result<T>> AndAsync<T>(this Task<Result> source,
        Func<Task<Result<T>>> otherFunc)
    {
        var currentSource = source.Result;
        var current = currentSource as Result<T> ?? new Result<T>(currentSource.HttpStatusCode)
        {
            Messages = currentSource.Messages
        };

        if (current.HasErrors) 
            return current;

        var r1 = await otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        current.OverrideDataIfNecessary(r1);
        return current;
    }

    public static async Task<Result<T>> AndAsync<T>(this Task<Result> source,
       Func<Result<T>> otherFunc)
    {
        var sourceResult = source.Result;
        var current = sourceResult as Result<T> ?? new Result<T>(sourceResult.HttpStatusCode)
        {
            Messages = sourceResult.Messages
        };

        if (current.HasErrors)
            return await Task.FromResult(current);

        var r1 = otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        current.OverrideDataIfNecessary(r1);
        return await Task.FromResult(current);
    }

    public static async Task<Result<T>> AndAsync<T>(this Task<Result<T>> current,
        Func<Result> otherFunc)
    {
        var currentResult = current.Result;
        if (currentResult.HasErrors)
            return await current;

        var r1 = otherFunc();
        currentResult.HttpStatusCode = r1.HttpStatusCode;
        currentResult.Messages.AddRange(r1.Messages);
        return await current;
    }

    public static async Task<Result<T>> AndAsync<T>(this Task<Result<T>> current,
        Func<Result<T>> otherFunc)
    {
        var currentResult = current.Result;
        if (currentResult.HasErrors)
            return await current;

        var r1 = otherFunc();
        currentResult.HttpStatusCode = r1.HttpStatusCode;
        currentResult.Messages.AddRange(r1.Messages);
        currentResult.OverrideDataIfNecessary(r1);

        return await current;
    }
    
    public static async Task<Result<T>> AndAsync<T>(this Task<Result<T>> current,
        Func<T, Result> otherFunc)
    {
        var currentResult = current.Result;
        if (currentResult.HasErrors)
            return await current;

        var r1 = otherFunc(currentResult.Data);
        currentResult.HttpStatusCode = r1.HttpStatusCode;
        currentResult.Messages.AddRange(r1.Messages);
        if (r1 is Result<T> rr)
            currentResult.OverrideDataIfNecessary(rr);

        return await current;
    }

    public static async Task<Result<TM>> AndAsync<T, TM>(this Task<Result<T>> current,
        Func<T, Result<TM>> otherFunc)
    {
        var currentResult = current.Result;
        var result = currentResult as Result<TM> ?? new Result<TM>(currentResult.HttpStatusCode)
        {
            Messages = currentResult.Messages
        };
        if (result.Success)
            result = result.And(() => otherFunc(currentResult.Data));

        return await Task.FromResult(result);
    }

    public static async Task<Result<TM>> AndAsync<T, TM>(this Task<Result<T>> current,
        Func<T, Task<Result<TM>>> otherFunc)
    {
        var currentResult = current.Result;
        var result = currentResult as Result<TM> ?? new Result<TM>(currentResult.HttpStatusCode)
        {
            Messages = currentResult.Messages
        };
        if (!result.Success)
            return await Task.FromResult(result);

        var res = await otherFunc(currentResult.Data);
        result = result.And(() => res);

        return await Task.FromResult(result);
    }

    public static async Task<Result> AndAsync<T>(this Task<Result<T>> current,
        Func<T, Task<Result>> otherFunc)
    {
        var result = current.Result;
        if (result == null)
            return await Task.FromResult((Result<T>)null);

        if (!result.Success)
            return await Task.FromResult(result);

        var res = await otherFunc(result.Data);
        result = result.And(() => res);
        result.RemoveData();

        return await Task.FromResult(result);
    }

    public static async Task<Result<T>> WithDataAsync<TI, T>(this Task<Result> current,
        Func<TI> dataFunc)
        where T : class
    {
        var currentResult = current.Result;
        var result = currentResult as Result<T> ?? new Result<T>(currentResult.HttpStatusCode)
        {
            Messages = currentResult.Messages
        };
        if (result.HasErrors)
            return await Task.FromResult(result);

        var data = dataFunc();
        result.Data = data as T;
        return await Task.FromResult(result);
    }

    public static async Task<Result<T>> WithDataAsync<T>(this Task<Result> current,
        Func<T> dataFunc)
    {
        var currentResult = current.Result;
        var result = currentResult as Result<T> ?? new Result<T>(currentResult.HttpStatusCode)
        {
            Messages = currentResult.Messages
        };
        if (result.HasErrors)
            return await Task.FromResult(result);

        var data = dataFunc();
        result.Data = data;
        return await Task.FromResult(result);
    }

    public static async Task<Result> RemoveDataAsync(this Task<Result> current)
    {
        var currentResult = current.Result;
        if (currentResult.GetType().IsConstructedGenericType)
            return await Task.FromResult(new Result(currentResult.HttpStatusCode)
            {
                Messages = currentResult.Messages
            });

        return await current;
    }

    public static async Task<Result> RemoveDataAsync<T>(this Task<Result<T>> current)
    {
        var currentResult = current.Result;
        if (currentResult.GetType().IsConstructedGenericType)
            return await Task.FromResult(new Result(currentResult.HttpStatusCode)
            {
                Messages = currentResult.Messages
            });

        return await current;
    }

    public static async Task<Result<T>> PipeAsync<T>(this Task<Result<T>> current,
        Action<T> pipeAction)
    {
        var result = current.Result;
        if (result.Success)
            pipeAction(result.Data);

        return await current;
    }

    public static async Task<Result> PipeAsync(this Task<Result> current,
        Action pipeAction)
    {
        var result = current.Result;
        if (result.Success) pipeAction();

        return await current;
    }
}