using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace UniTutor
{
    public class PasswordHash
    {
        // Hashes the provided password using PBKDF2 with a random salt
        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA1 with 10000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combine salt and hashed password
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Verifies if the provided password matches the hashed password
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Extract salt and hashed password from the stored hashed password
                string[] parts = hashedPassword.Split(':');
                byte[] salt = Convert.FromBase64String(parts[0]);
                string storedHash = parts[1];

                // Hash the provided password with the extracted salt
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                // Compare the computed hash with the stored hash
                return storedHash == hashed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying password: {ex.Message}");
                return false;
            }
        }
    }
}
