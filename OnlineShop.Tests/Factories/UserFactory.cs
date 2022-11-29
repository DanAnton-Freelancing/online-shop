using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories
{
    public static class UserFactory
    {
        public static string GetSecret() 
            => "Umrv5pYQHpU79RAh9psE5VwuDtWtAysafugUcQjRF4nAAXPxnjmhNt" +
               "6PeeY7xLsqdvMnWBKYcMjS3uzVCPJh9cfbjqhdYhyxDdXyU2fEH4x4DnGdPEvye" +
               "QCA9HCvTKZhBBWsB5rWC3uSqLL6PfLAqLH6gNzeNRXfkpPscpm9CYBSxyshDmxsdm" +
               "UBLdsZ3Wfk2KsDAL6C9tNdtmLby6f5gWcf8PKY8SjKwGXua9XMbFLsfn4t3HHF67rANxuYybac";

        public static secondaryPorts.User ToEntity(this secondaryPorts.User user)
        {
            user.Id = Guid.NewGuid();
            return user;
        }

        public static secondaryPorts.User Create()
            => new()
            {
                Email = "user1@example.com",
                FirstName = "user1",
                LastName = "user1",
                UserCart = new secondaryPorts.UserCart(),
                Password = "Centric1!",
                Username = "user1"
            };

        public static secondaryPorts.UserCart CreateUserCart()
            => new()
            {
                CartItems = new List<secondaryPorts.CartItem>()
            };

        public static primaryPorts.UserCart CreateUserDetails()
            => new()
            {
                Id = Guid.NewGuid(),
                CartItems = new List<primaryPorts.CartItem>(),
                Total = 0
            };

        public static void ToEntity(this primaryPorts.UserCart userCart, Guid id)
            => userCart.Id = id;

        public static void ToEntity(this secondaryPorts.UserCart userCart)
            => userCart.Id = Guid.NewGuid();

        public static primaryPorts.RegisterUser CreateRegisterUser()
            => new()
            {
                Email = "user1@example.com",
                FirstName = "user1",
                LastName = "user1",
                Password = "Centric1!",
                Username = "user1"
            };

        public static primaryPorts.LoginUser CreateLoginUser()
            => new()
            {
                Password = "Centric1!",
                Username = "user1"
            };

        public static void AddSalt(this secondaryPorts.User user)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            user.Salt = salt;
        }

        public static void AddPasswordHash(this secondaryPorts.User user)
            => user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(user.Password,
                                                                           user.Salt,
                                                                           KeyDerivationPrf.HMACSHA1,
                                                                           10000,
                                                                           256 / 8));
    }
}