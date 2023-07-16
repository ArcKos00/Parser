using System.Security.Cryptography;

namespace Utils
{
    public class MD5HashCreator
    {
        public static async Task<string> GetImageHash(Stream stream)
        {
            using (var md = new MD5CryptoServiceProvider())
            {
                var hash = await md.ComputeHashAsync(stream);
                return BitConverter.ToString(hash).Replace("-", "") + ".png";
            }
        }
    }
}
