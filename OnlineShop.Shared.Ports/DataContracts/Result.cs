using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Shared.Ports.DataContracts;

public sealed class Result<T> : Result
{
    public Result() { }

    public Result(HttpStatusCode statusCode) : base(statusCode) { }

    public T Data { get; set; }
}

public class Result
{
    public Result() { }

    public Result(HttpStatusCode statusCode)
    {
        if ((int) statusCode < 100) throw new ArgumentException($"Invalid status code {statusCode}");

        HttpStatusCode = statusCode;
    }

    public HttpStatusCode HttpStatusCode { get; set; }
    public List<Message> Messages { get; set; } = new();
    public bool HasErrors => Messages.Any(m => m.MessageType >= MessageType.Error);
    public bool Success => !HasErrors;
    public string ErrorMessage => Messages?.FirstOrDefault(m => m.MessageType >= MessageType.Error)?.ResourceKey;

    public static Result Ok() => new(HttpStatusCode.OK);

    public static Result Ok(HttpStatusCode code) => new(code);

    public static Result<T> Ok<T>(T data) => new(HttpStatusCode.OK)
    {
        Data = data
    };

    public static Result<T> Ok<T>(T data, HttpStatusCode code) => new(code)
    {
        Data = data
    };

    public static Result Warning(string resourceKey,
        params string[] messageParams)
    {
        var result = Ok();
        result.Add(MessageType.Warning, resourceKey, messageParams);
        return result;
    }

    public static Result<T> Warning<T>(T data,
        string resourceKey,
        params string[] messageParams)
    {
        var result = Ok(data);
        result.Add(MessageType.Warning, resourceKey, messageParams);
        return result;
    }

    public static Result Error(HttpStatusCode statusCode,
        string resourceKey,
        params string[] messageParams)
        => Error(statusCode, MessageType.Error, resourceKey, messageParams);

    public static Result<T> Error<T>(HttpStatusCode statusCode,
        string resourceKey,
        params string[] messageParams)
        => Error<T>(statusCode, MessageType.Error, resourceKey, messageParams);

    public static Result Error(HttpStatusCode statusCode,
        MessageType type,
        string resourceKey,
        params string[] messageParams)
    {
        var result = new Result(statusCode);
        result.Add(type, resourceKey, messageParams);
        return result;
    }

    public static Result<T> Error<T>(HttpStatusCode statusCode,
        MessageType messageType,
        string resourceKey,
        params string[] messageParams)
    {
        var result = new Result<T>(statusCode);
        result.Add(messageType, resourceKey, messageParams);
        return result;
    }

    public static Result<T> Error<T>(T data,
        HttpStatusCode statusCode,
        string resourceKey,
        params string[] messageParams)
    {
        var result = Error<T>(statusCode, MessageType.Error, resourceKey, messageParams);
        result.Data = data;
        return result;
    }

    public override string ToString()
    {
        var messagesAsString = string.Join(Environment.NewLine, Messages);
        return $"[{HttpStatusCode}] - [Success:{Success}] - [Messages:{Environment.NewLine}{messagesAsString}]";
    }
}