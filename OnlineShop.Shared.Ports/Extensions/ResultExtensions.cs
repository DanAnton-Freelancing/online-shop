using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Exceptions;

namespace OnlineShop.Shared.Ports.Extensions;

public static class ResultExtensions
{
    public static Result And(this Result current,
        Func<Result> otherFunc)
    {
        if (current.HasErrors) return current;

        var r1 = otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        return current;
    }

    public static Result<T> WithData<TI, T>(this Result current,
        Func<TI> dataFunc)
        where T : class
    {
        var result = current as Result<T> ?? new Result<T>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        if (result.HasErrors) return result;

        var data = dataFunc();
        result.Data = data as T;
        return result;
    }

    public static Result<T> WithData<T>(this Result current,
        Func<T> dataFunc)
    {
        var result = current as Result<T> ?? new Result<T>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        if (result.HasErrors) return result;

        var data = dataFunc();
        result.Data = data;
        return result;
    }

    public static Result RemoveData(this Result current)
    {
        if (current.GetType().IsConstructedGenericType)
            return new Result(current.HttpStatusCode)
            {
                Messages = current.Messages
            };

        return current;
    }

    public static Result<T> And<T>(this Result source,
        Func<Result<T>> otherFunc)
    {
        var current = source as Result<T> ?? new Result<T>(source.HttpStatusCode)
        {
            Messages = source.Messages
        };

        if (current.HasErrors) return current;

        var r1 = otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        current.OverrideDataIfNecessary(r1);
        return current;
    }

    public static Result<T> And<T>(this Result<T> current,
        Func<Result> otherFunc)
    {
        if (current.HasErrors) return current;

        var r1 = otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        return current;
    }

    public static Result<T> And<T>(this Result<T> current,
        Func<Result<T>> otherFunc)
    {
        if (current.HasErrors) return current;

        var r1 = otherFunc();
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        current.OverrideDataIfNecessary(r1);

        return current;
    }

    public static Result<T> And<T>(this Result<T> current,
        Func<T, Result> otherFunc)
    {
        if (current.HasErrors) return current;

        var r1 = otherFunc(current.Data);
        current.HttpStatusCode = r1.HttpStatusCode;
        current.Messages.AddRange(r1.Messages);
        if (r1 is Result<T> rr) current.OverrideDataIfNecessary(rr);

        return current;
    }

    public static Result<TM> And<T, TM>(this Result<T> current,
        Func<T, Result<TM>> otherFunc)
    {
        var result = current as Result<TM> ?? new Result<TM>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        if (result.Success) result = result.And(() => otherFunc(current.Data));

        return result;
    }

    public static Result OnError(this Result current,
        Func<Result, Result> otherFunc)
        => current.Success ? current : otherFunc(current);

    public static Result OnError(this Result current,
        Func<Result> otherFunc)
        => current.Success ? current : otherFunc();

    public static Result<T> OnError<T>(this Result<T> current,
        Func<Result<T>, Result<T>> otherFunc)
        => current.Success ? current : otherFunc(current);

    public static Result<TM> Finally<T, TM>(this Result<T> current,
        Func<T, Result<TM>> otherFunc)
    {
        var result = current as Result<TM> ?? new Result<TM>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        var r1 = otherFunc(current.Data);

        result.Messages.AddRange(r1.Messages);
        result.Data = r1.Data;
        result.HttpStatusCode = result.Success ? r1.HttpStatusCode : result.HttpStatusCode;
        return result;
    }

    public static Result<T> Finally<T>(this Result<T> current,
        Func<T, Result> otherFunc)
    {
        var result = current;
        var r1 = otherFunc(current.Data);

        result.Messages.AddRange(r1.Messages);
        result.HttpStatusCode = result.Success ? r1.HttpStatusCode : result.HttpStatusCode;

        return result;
    }

    public static Result<TM> Map<T, TM>(this Result<T> current,
        Func<T, TM> mappingFunc)
    {
        var result = current as Result<TM> ?? new Result<TM>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        if (result.Success) result.Data = mappingFunc(current.Data);

        return result;
    }

    public static async Task<Result<TM>> MapAsync<T, TM>(this Task<Result<T>> current,
        Func<T, TM> mappingFunc)
    {
        var currentResult = current.Result;
        var result = currentResult as Result<TM> ?? new Result<TM>(currentResult.HttpStatusCode)
        {
            Messages = currentResult.Messages
        };
        if (result.Success) result.Data = mappingFunc(currentResult.Data);

        return await Task.FromResult(result);
    }

    public static Result<PaginatedData<TM>> Map<T, TM>(this Result<PaginatedData<T>> current,
        Func<List<T>, List<TM>> mappingFunc)
    {
        var result = new Result<PaginatedData<TM>>(current.HttpStatusCode)
        {
            Messages = current.Messages
        };
        if (result.Success)
            result.Data = new PaginatedData<TM>
            {
                Data = mappingFunc(current.Data.Data),
                Offset = current.Data.Offset,
                Limit = current.Data.Limit,
                Total = current.Data.Total
            };

        return result;
    }

    public static Result<T> Pipe<T>(this Result<T> current,
        Action<T> pipeAction)
    {
        var result = current;
        if (result.Success) pipeAction(result.Data);

        return result;
    }

    public static Result Pipe(this Result current,
        Action pipeAction)
    {
        var result = current;
        if (result.Success) pipeAction();

        return result;
    }

    public static void Add(this Result current,
        List<Message> messages)
    {
        current.Messages.AddRange(messages);
        if (current.HttpStatusCode == HttpStatusCode.OK
            && messages.Any(m => m.MessageType >= MessageType.Error))
            current.HttpStatusCode = messages.Max(m => m.MessageType).GetDefaultStatusCode();
    }

    public static void Add(this Result current,
        MessageType type,
        string resourceKey,
        params string[] messageParams)
    {
        if (current.HttpStatusCode == HttpStatusCode.OK
            && type >= MessageType.Error)
            current.HttpStatusCode = type.GetDefaultStatusCode();

        current.Messages.Add(new Message
        {
            MessageType = type,
            ResourceKey = resourceKey,
            MessageParams = messageParams?.ToList()
        });
    }

    public static void EnsureSuccess(this Result result)
    {
        var sanitizedResult = result
                              ?? Result.Error(HttpStatusCode.InternalServerError, MessageType.Error, "[NoResult]");
        if (sanitizedResult.HasErrors) throw new ResultException(sanitizedResult);
    }

    internal static void OverrideDataIfNecessary<T>(this Result<T> current,
        Result<T> other)
    {
        if (other.Success
            || current.Data?.Equals(default(T)) == true)
            current.Data = other.Data;
    }

    private static HttpStatusCode GetDefaultStatusCode(this MessageType messageType)
    {
        return messageType switch
        {
            MessageType.Debug => HttpStatusCode.OK,
            MessageType.Info => HttpStatusCode.OK,
            MessageType.Warning => HttpStatusCode.OK,
            MessageType.Confirmation => HttpStatusCode.OK,
            MessageType.Error => HttpStatusCode.InternalServerError,
            MessageType.OtherError => HttpStatusCode.InternalServerError,
            MessageType.ConcurrencyError => HttpStatusCode.Conflict,
            MessageType.ValidationError => HttpStatusCode.BadRequest,
            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
}