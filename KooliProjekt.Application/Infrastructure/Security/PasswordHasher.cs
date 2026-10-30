using System;
using System.Security.Cryptography;

namespace KooliProjekt.Application.Infrastructure.Security
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
        }
    }
}
