using System.Security.Cryptography;
using System.Text;

namespace QLbansach_tutorial.Helpers;

public static class Helper
{
    public static string hashpassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
} 