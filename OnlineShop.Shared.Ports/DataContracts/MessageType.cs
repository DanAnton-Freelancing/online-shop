namespace OnlineShop.Shared.Ports.DataContracts;

public enum MessageType
{
    Debug,
    Info,
    Warning,
    Confirmation,
    Error,
    OtherError,
    ConcurrencyError,
    ValidationError
}