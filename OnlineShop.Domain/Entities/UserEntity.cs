using OnlineShop.Domain.Aggregate;
using OnlineShop.Domain.Shared;

namespace OnlineShop.Domain.Entities;

public class UserEntity : EditableEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }

    public virtual UserCartEntity UserCartEntity { get; set; }
}