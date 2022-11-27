using System;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Shared.Ports.Exceptions
{
    public class ResultException : Exception
    {
        public readonly Result Result;

        public ResultException(Result result) : base(result.ErrorMessage)
            => Result = result;

        public override string ToString()
            => $"{Result}{Environment.NewLine}{base.ToString()}";
    }
}