using OnlineShop.Domain.Entities;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.Mappers;

public static class UserMapper
{
    public static UserEntity MapToDomain(this User user)
        => new()
        {
            Email = user.Email,
            Username = user.Username,
            Password = user.Password,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Salt = user.Salt,
            Version = user.Version
        };

    public static User MapToPorts(this UserEntity userEntity)
        => new()
        {
            Email = userEntity.Email,
            Username = userEntity.Username,
            Password = userEntity.Password,
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            Salt = userEntity.Salt,
            Version = userEntity.Version

        };

}