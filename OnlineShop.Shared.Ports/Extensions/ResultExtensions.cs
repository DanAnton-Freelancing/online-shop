using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Shared.Ports.Extensions;

public static class ResultExtensions
{

    public static Result RemoveData(this Result current)
    {
        if (current.GetType().IsConstructedGenericType)
            return new Result(current.HttpStatusCode)
            {
                Messages = current.Messages
            };

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