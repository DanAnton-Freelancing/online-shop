using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories;

public static class UserFactory
{
    public static string GetSecret() 
        => "Umrv5pYQHpU79RAh9psE5VwuDtWtAysafugUcQjRF4nAAXPxnjmhNt" +
           "6PeeY7xLsqdvMnWBKYcMjS3uzVCPJh9cfbjqhdYhyxDdXyU2fEH4x4DnGdPEvye" +
           "QCA9HCvTKZhBBWsB5rWC3uSqLL6PfLAqLH6gNzeNRXfkpPscpm9CYBSxyshDmxsdm" +
           "UBLdsZ3Wfk2KsDAL6C9tNdtmLby6f5gWcf8PKY8SjKwGXua9XMbFLsfn4t3HHF67rANxuYybac";

    public static User ToEntity(this User userDb)
    {
        userDb.Id = Guid.NewGuid();
        return userDb;
    }

    public static User Create()
        => new()
        {
            Email = "user1@example.com",
            FirstName = "user1",
            LastName = "user1",
            UserCart = new UserCart(),
            Password = "Centric1!",
            Username = "user1"
        };

    public static UserCart CreateUserCart()
        => new()
        {
            CartItems = new List<CartItem>()
        };

    public static primaryPorts.UserCartModel CreateUserDetails()
        => new()
        {
            Id = Guid.NewGuid(),
            CartItems = new List<primaryPorts.CartItemModel>(),
            Total = 0
        };

    public static void ToEntity(this primaryPorts.UserCartModel userCartModel, Guid id)
        => userCartModel.Id = id;

    public static void ToEntity(this UserCart userCart)
        => userCart.Id = Guid.NewGuid();

    public static primaryPorts.RegisterUserModel CreateRegisterUser()
        => new()
        {
            Email = "user1@example.com",
            FirstName = "user1",
            LastName = "user1",
            Password = "Centric1!",
            Username = "user1"
        };

    public static primaryPorts.LoginUserModel CreateLoginUser()
        => new()
        {
            Password = "Centric1!",
            Username = "user1"
        };

    public static void AddSalt(this User userDb)
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        userDb.Salt = salt;
    }

    public static void AddPasswordHash(this User userDb)
        => userDb.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(userDb.Password,
            userDb.Salt,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8));
}