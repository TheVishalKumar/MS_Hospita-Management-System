using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace HospitalManagementSystem.Services.Security
{
    /// <summary>
    /// Multi-Factor Authentication Service using Time-based One-Time Password (TOTP)
    /// Generates and verifies 6-digit codes every 30 seconds
    /// Used for Admin account security
    /// </summary>
    public interface IMfaService
    {
        /// <summary>
        /// Generate a new MFA secret for user account setup
        /// </summary>
        MfaSetupResult GenerateMfaSecret(string userEmail);

        /// <summary>
        /// Verify MFA token (6-digit code) against stored secret
        /// </summary>
        bool VerifyMfaToken(string base32Secret, string token);

        /// <summary>
        /// Generate QR code URL for scanning with authenticator app
        /// </summary>
        string GenerateQrCodeUrl(string userEmail, string base32Secret, string issuer = "HMS");

        /// <summary>
        /// Generate recovery codes for account recovery if authenticator is lost
        /// </summary>
        List<string> GenerateRecoveryCodes(int count = 10);

        /// <summary>
        /// Verify recovery code is valid and remove from list
        /// </summary>
        bool VerifyAndRemoveRecoveryCode(ref List<string> codes, string code);
    }

    public class MfaService : IMfaService
    {
        private const int TimeStep = 30; // 30 seconds for TOTP
        private const int CodeLength = 6; // 6-digit codes

        /// <summary>
        /// Generate a new MFA secret for user to set up authenticator
        /// </summary>
        public MfaSetupResult GenerateMfaSecret(string userEmail)
        {
            // Generate random 20-byte secret (160 bits for 6-digit codes)
            var secret = new byte[20];
            RandomNumberGenerator.Fill(secret);

            var base32Secret = ConvertToBase32(secret);

            return new MfaSetupResult
            {
                Secret = base32Secret,
                QrCodeUrl = GenerateQrCodeUrl(userEmail, base32Secret),
                RecoveryCodes = GenerateRecoveryCodes(10),
                ManualEntryKey = base32Secret
            };
        }

        /// <summary>
        /// Verify 6-digit code from authenticator app
        /// Allows ±1 time window for clock skew (acceptable ±30 seconds)
        /// </summary>
        public bool VerifyMfaToken(string base32Secret, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || token.Length != CodeLength)
                return false;

            if (!int.TryParse(token, out int codeValue))
                return false;

            try
            {
                var secretBytes = ConvertFromBase32(base32Secret);
                var now = DateTimeOffset.UtcNow;

                // Check current time window
                if (VerifyTotp(secretBytes, codeValue, now))
                    return true;

                // Check previous time window (time drift compensation)
                if (VerifyTotp(secretBytes, codeValue, now.AddSeconds(-TimeStep)))
                    return true;

                // Check next time window (very lenient for slow users)
                if (VerifyTotp(secretBytes, codeValue, now.AddSeconds(TimeStep)))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generate QR code URL for authenticator app scanning
        /// Format: otpauth://totp/HMS:email@example.com?secret=XXXXX&issuer=HMS
        /// </summary>
        public string GenerateQrCodeUrl(string userEmail, string base32Secret, string issuer = "HMS")
        {
            var query = System.Web.HttpUtility.UrlEncode(
                $"otpauth://totp/{issuer}:{userEmail}?secret={base32Secret}&issuer={issuer}");
            
            // Using Google Charts API for QR code generation
            return $"https://chart.googleapis.com/chart?chs=200x200&chld=M|0&cht=qr&chl={query}";
        }

        /// <summary>
        /// Generate recovery codes for account recovery
        /// Format: XXXX-XXXX (4 groups of 4 hex characters)
        /// </summary>
        public List<string> GenerateRecoveryCodes(int count = 10)
        {
            var codes = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var codeBytes = new byte[6]; // 12 hex characters
                RandomNumberGenerator.Fill(codeBytes);
                var code = BitConverter.ToString(codeBytes).Replace("-", "").ToUpper();
                
                // Format as XXXX-XXXX
                var formatted = $"{code.Substring(0, 4)}-{code.Substring(4, 4)}";
                codes.Add(formatted);
            }
            return codes;
        }

        /// <summary>
        /// Verify and remove recovery code from list
        /// </summary>
        public bool VerifyAndRemoveRecoveryCode(ref List<string> codes, string code)
        {
            if (string.IsNullOrWhiteSpace(code) || codes == null || !codes.Any())
                return false;

            var normalizedCode = code.Trim().ToUpper();
            var index = codes.FirstOrDefault(c => c.ToUpper() == normalizedCode);

            if (index != null)
            {
                codes.Remove(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify TOTP code for given timestamp
        /// Uses HMAC-SHA1 to generate code
        /// </summary>
        private bool VerifyTotp(byte[] secretBytes, int expectedCode, DateTimeOffset now)
        {
            var counter = (long)(now.ToUnixTimeSeconds() / TimeStep);
            var counterBytes = BitConverter.GetBytes(counter);
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            using (var hmac = new System.Security.Cryptography.HMACSHA1(secretBytes))
            {
                var hash = hmac.ComputeHash(counterBytes);
                var offset = hash[hash.Length - 1] & 0x0f;
                var code = ((hash[offset] & 0x7f) << 24 |
                           (hash[offset + 1] & 0xff) << 16 |
                           (hash[offset + 2] & 0xff) << 8 |
                           (hash[offset + 3] & 0xff)) % (int)Math.Pow(10, CodeLength);

                return code == expectedCode;
            }
        }

        /// <summary>
        /// Convert byte array to Base32 string (RFC 4648)
        /// </summary>
        private static string ConvertToBase32(byte[] input)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var sb = new StringBuilder();
            int bits = 0;
            int value = 0;

            foreach (var b in input)
            {
                value = (value << 8) | b;
                bits += 8;

                while (bits >= 5)
                {
                    bits -= 5;
                    sb.Append(alphabet[(value >> bits) & 31]);
                }
            }

            if (bits > 0)
            {
                sb.Append(alphabet[(value << (5 - bits)) & 31]);
            }

            // Add padding
            while ((sb.Length % 8) != 0)
            {
                sb.Append('=');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert Base32 string back to byte array
        /// </summary>
        private static byte[] ConvertFromBase32(string input)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var output = new List<byte>();
            int bits = 0;
            int value = 0;

            foreach (var c in input)
            {
                if (c == '=')
                    break;

                value = (value << 5) | alphabet.IndexOf(char.ToUpper(c));
                bits += 5;

                if (bits >= 8)
                {
                    bits -= 8;
                    output.Add((byte)((value >> bits) & 255));
                }
            }

            return output.ToArray();
        }
    }

    /// <summary>
    /// Result object returned when setting up MFA
    /// </summary>
    public class MfaSetupResult
    {
        /// <summary>
        /// Base32 encoded secret key for authenticator app
        /// </summary>
        public string? Secret { get; set; }

        /// <summary>
        /// QR code URL for scanning with authenticator
        /// </summary>
        public string? QrCodeUrl { get; set; }

        /// <summary>
        /// Recovery codes in case authenticator is lost
        /// </summary>
        public List<string>? RecoveryCodes { get; set; }

        /// <summary>
        /// Manual entry key for manual input into authenticator
        /// </summary>
        public string? ManualEntryKey { get; set; }
    }
}
