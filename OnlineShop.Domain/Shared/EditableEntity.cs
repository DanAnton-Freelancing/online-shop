namespace OnlineShop.Domain.Shared;

public class EditableEntity : BaseEntity
{
    public byte[] Version { get; set; }
}