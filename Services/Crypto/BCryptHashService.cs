using BCrypt.Net;

namespace BackendServiceStarter.Services.Crypto
{
    public class BCryptHashService : IHashService
    {
        public string Hash(string data)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(data, HashType.SHA512, 16);
        }

        public bool Verify(string data, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(data, hash, HashType.SHA512);
        }
    }
}