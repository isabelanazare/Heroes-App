using Fabrit.Heroes.Business.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fabrit.Heroes.Business.Services
{
    public class HashingService : IHashingService
    {
        private const string PASSWORD_SALT = "QwErTy1@3$5";
        private const int PASSWORD_MIN_LENGTH = 5;

        public int GetPasswordMinLength() => PASSWORD_MIN_LENGTH;

        public string GetHashedPassword(string password)
        {
            using var sha512 = SHA512.Create();

            var bytes = sha512.ComputeHash(
                Encoding.UTF8.GetBytes(password + PASSWORD_SALT)
            );

            return bytes
                .Select(x => x.ToString("X2"))
                .Aggregate((acc, x) => acc + x);
        }
    }
}
