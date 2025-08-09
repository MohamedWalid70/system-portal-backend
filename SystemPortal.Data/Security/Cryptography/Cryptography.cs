using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SystemPortal.Data.Security.Cryptography
{
    public class Cryptography
    {
        public static async ValueTask<string> GetPasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                
                Stream passwordStream = new MemoryStream(bytes);

                byte[] hash = await sha256.ComputeHashAsync(passwordStream);

                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
