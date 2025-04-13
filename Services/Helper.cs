using System.Security.Cryptography;
using System.Text;

namespace QLbansach_tutorial.Services;

public static class Helper
{
    public static string hashpassword(string password)
    {
        HashAlgorithm hash = SHA256.Create();
        
        return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(password)));
    }
}