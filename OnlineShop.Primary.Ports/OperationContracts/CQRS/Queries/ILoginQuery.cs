using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries
{
    public interface ILoginQuery : IQuery<Result<string>>
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}