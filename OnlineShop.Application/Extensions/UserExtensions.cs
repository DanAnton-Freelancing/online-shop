using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Domain.Entities;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Application.Extensions;

public static class UserExtensions
{
    public static Result<UserEntity> HashPassword(this UserEntity userEntityDb)
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        userEntityDb.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(userEntityDb.Password,
            salt,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8));
        userEntityDb.Salt = salt;

        return Result.Ok(userEntityDb);
    }

    public static Result<UserEntity> CheckPasswordHash(this UserEntity userEntityDb, string password)
    {
        var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password,
            userEntityDb.Salt,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8));
        return hash.Equals(userEntityDb.Password)
            ? Result.Ok(userEntityDb)
            : Result.Error<UserEntity>(HttpStatusCode.NotFound, "[NotFound]",
                ErrorMessages.UsernameOrPasswordNotFound);
    }

    public static Result<string> GenerateToken(this UserEntity userEntityDb, string secret)
        => Result.Ok(GenerateTokenInternal(userEntityDb, secret));


    private static string GenerateTokenInternal(UserEntity userEntityDb, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userEntityDb.Id.ToString()),
                new Claim(ClaimTypes.Email, userEntityDb.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}