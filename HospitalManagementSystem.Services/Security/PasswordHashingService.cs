using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Security
{
    /// <summary>
    /// Password hashing service using BCrypt algorithm
    /// Provides secure password hashing and verification
    /// 
    /// IMPORTANT: Requires NuGet package: BCrypt.Net-Next
    /// Install via: dotnet add package BCrypt.Net-Next
    /// </summary>
    public class PasswordHashingService : IPasswordHashingService
    {
        /// <summary>
        /// Hashes a plain text password using BCrypt with cost factor 12
        /// BCrypt automatically includes salt in the output
        /// </summary>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>Hashed password (includes salt)</returns>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            if (password.Length > 72)
            {
                throw new ArgumentException("Password cannot exceed 72 characters (BCrypt limitation)", nameof(password));
            }

            try
            {
                // BCrypt.Net.BCrypt.HashPassword(password, workFactor)
                // workFactor default is 10, using 12 for more security (slower hashing = harder to brute force)
                return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password", ex);
            }
        }

        /// <summary>
        /// Verifies a plain text password against a BCrypt hash
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="hash">BCrypt hash to compare against</param>
        /// <returns>True if password matches hash, False otherwise</returns>
        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentException("Hash cannot be empty", nameof(hash));
            }

            try
            {
                // BCrypt automatically handles salt comparison
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying password", ex);
            }
        }

        /// <summary>
        /// Generates a random strong password for temporary user accounts
        /// Useful when creating new employee or doctor accounts
        /// </summary>
        /// <param name="length">Length of password (default 16, min 12, max 32)</param>
        /// <returns>Random strong password</returns>
        public string GenerateRandomPassword(int length = 16)
        {
            if (length < 12)
                throw new ArgumentException("Password length should be at least 12", nameof(length));
            if (length > 72)
                throw new ArgumentException("Password length cannot exceed 72 (BCrypt limitation)", nameof(length));

            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|;:',.<>?";
            var random = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => validChars[random.Next(validChars.Length)])
                .ToArray());
        }
    }

    /// <summary>
    /// Interface for password hashing service
    /// Allows for different implementations or testing with mocks
    /// </summary>
    public interface IPasswordHashingService
    {
        /// <summary>
        /// Hashes a plain text password
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plain text password against a hash
        /// </summary>
        bool VerifyPassword(string password, string hash);

        /// <summary>
        /// Generates a random strong password
        /// </summary>
        string GenerateRandomPassword(int length = 16);
    }
}
