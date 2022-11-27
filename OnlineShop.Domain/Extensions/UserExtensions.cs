using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Domain.Extensions
{
    public static class UserExtensions
    {
        public static Result<User> HashPassword(this User user)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }

            user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(user.Password,
                                                                        salt,
                                                                        KeyDerivationPrf.HMACSHA1,
                                                                        10000,
                                                                        256 / 8));
            user.Salt = salt;

            return Result.Ok(user);
        }

        public static Result<User> CheckPasswordHash(this User user, string password)
        {
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password,
                                                                   user.Salt,
                                                                   KeyDerivationPrf.HMACSHA1,
                                                                   10000,
                                                                   256 / 8));
            return hash.Equals(user.Password)
                       ? Result.Ok(user)
                       : Result.Error<User>(HttpStatusCode.NotFound, "[NotFound]",
                                            ErrorMessages.UsernameOrPasswordNotFound);
        }

        public static Result<string> GenerateToken(this User user, string secret)
            => Result.Ok(GenerateTokenInternal(user, secret));


        private static string GenerateTokenInternal(User user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
                                  {
                                      Subject = new ClaimsIdentity(new[]
                                                                   {
                                                                       new Claim(ClaimTypes.Name, user.Id.ToString()),
                                                                       new Claim(ClaimTypes.Email, user.Email)
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
}