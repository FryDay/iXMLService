using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Tester
{
    public static class LoginInfo
    {
        public static string URL { get; set; }
        public static string User { get; set; }
        private static byte[] Password;

        public static void SetPassword(string password)
        {
            Password = Encoding.ASCII.GetBytes(password.PadRight(16, ' '));

            EncryptInMemoryData(Password, MemoryProtectionScope.SameProcess);
        }

        public static string GetPassword()
        {
            byte[] decPass = new byte[Password.Length];
            Password.CopyTo(decPass, 0);

            DecryptInMemoryData(decPass, MemoryProtectionScope.SameProcess);
            return Encoding.ASCII.GetString(decPass).Trim();
        }

        public static void EncryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (buffer == null)
                throw new ArgumentNullException("Buffer");

            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(buffer, scope);
        }

        public static void DecryptInMemoryData(byte[] buffer, MemoryProtectionScope scope)
        {
            if (buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (buffer == null)
                throw new ArgumentNullException("Buffer");

            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(buffer, scope);
        }
    }
}
