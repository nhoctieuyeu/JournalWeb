using System;
using System.Security.Cryptography;
using System.Text;

namespace JournalWeb.Helpers
{
    public static class SecurityHelper
    {
        // ===== HASH PASSWORD =====
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // ===== HASH PIN =====
        public static string HashPin(string pin)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pin));
                return Convert.ToBase64String(bytes);
            }
        }

        // ===== VERIFY PIN (🔴 THIẾU HÀM NÀY GÂY LỖI BUILD) =====
        public static bool VerifyPin(string inputPin, string storedPinHash)
        {
            if (string.IsNullOrEmpty(inputPin) || string.IsNullOrEmpty(storedPinHash))
                return false;

            string hash = HashPin(inputPin);
            return hash == storedPinHash;
        }
    }
}
