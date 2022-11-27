using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using sp = OnlineShop.Secondary.Ports.DataContracts;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Factories
{
    public static class UserFactory
    {
        public static string GetSecret() 
            => "Umrv5pYQHpU79RAh9psE5VwuDtWtAysafugUcQjRF4nAAXPxnjmhNt" +
               "6PeeY7xLsqdvMnWBKYcMjS3uzVCPJh9cfbjqhdYhyxDdXyU2fEH4x4DnGdPEvye" +
               "QCA9HCvTKZhBBWsB5rWC3uSqLL6PfLAqLH6gNzeNRXfkpPscpm9CYBSxyshDmxsdm" +
               "UBLdsZ3Wfk2KsDAL6C9tNdtmLby6f5gWcf8PKY8SjKwGXua9XMbFLsfn4t3HHF67rANxuYybac";

        public static sp.User ToEntity(this sp.User user)
        {
            user.Id = Guid.NewGuid();
            return user;
        }

        public static sp.User Create()
            => new sp.User
            {
                Email = "user1@example.com",
                FirstName = "user1",
                LastName = "user1",
                UserCart = new sp.UserCart(),
                Password = "Centric1!",
                Username = "user1"
            };

        public static sp.UserCart CreateUserCart()
            => new sp.UserCart
            {
                CartItems = new List<sp.CartItem>()
            };

        public static pp.UserCart CreateUserDetails()
            => new pp.UserCart
            {
                Id = Guid.NewGuid(),
                CartItems = new List<pp.CartItem>(),
                Total = 0
            };

        public static void ToEntity(this pp.UserCart userCart, Guid id)
            => userCart.Id = id;

        public static void ToEntity(this sp.UserCart userCart)
            => userCart.Id = Guid.NewGuid();

        public static pp.RegisterUser CreateRegisterUser()
            => new pp.RegisterUser
            {
                Email = "user1@example.com",
                FirstName = "user1",
                LastName = "user1",
                Password = "Centric1!",
                Username = "user1"
            };

        public static pp.LoginUser CreateLoginUser()
            => new pp.LoginUser
            {
                Password = "Centric1!",
                Username = "user1"
            };

        public static void AddSalt(this sp.User user)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            user.Salt = salt;
        }

        public static void AddPasswordHash(this sp.User user)
            => user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(user.Password,
                                                                           user.Salt,
                                                                           KeyDerivationPrf.HMACSHA1,
                                                                           10000,
                                                                           256 / 8));
    }
}