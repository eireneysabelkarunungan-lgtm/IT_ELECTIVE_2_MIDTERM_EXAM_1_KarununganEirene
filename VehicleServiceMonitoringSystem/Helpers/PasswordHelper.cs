using System.Security.Cryptography;
using System.Text;

namespace VehicleServiceMonitoringSystem.Helpers
{

    public static class PasswordHelper
    {
        public static string Hash(string plainTextPassword)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plainTextPassword);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool Verify(string plainTextPassword, string hashedPassword)
        {
            var hashOfInput = Hash(plainTextPassword);
            return string.Equals(hashOfInput, hashedPassword, StringComparison.Ordinal);
        }
    }
}
