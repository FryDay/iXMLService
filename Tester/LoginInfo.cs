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
            DecryptInMemoryData(Password, MemoryProtectionScope.SameProcess);
            Console.WriteLine(Encoding.ASCII.GetString(Password));
            return Encoding.ASCII.GetString(Password).Trim();
        }

        public static void EncryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        {
            if (Buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (Buffer == null)
                throw new ArgumentNullException("Buffer");

            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(Buffer, Scope);
        }

        public static void DecryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        {
            if (Buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (Buffer == null)
                throw new ArgumentNullException("Buffer");

            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(Buffer, Scope);
        }
    }
}
