using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System;

namespace GestionDisponibilite.Service
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or whitespace.", nameof(password));

            return Argon2.Hash(password); // Encodé avec tous les paramètres automatiquement
        }

        public bool VerifyPassword(string hash, string password)
        {
            if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(password))
                return false;

            return Argon2.Verify(hash, password);
        }
    }
}


