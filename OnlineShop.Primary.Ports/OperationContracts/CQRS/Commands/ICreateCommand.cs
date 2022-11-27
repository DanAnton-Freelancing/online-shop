namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands
{
    public interface ICreateCommand<T, out TI> : ICommand<TI>
    {
        T Data { get; set; }
    }
}