namespace BackendServiceStarter.Services.Crypto
{
    public interface IHashService
    {
        public string Hash(string data);
        public bool Verify(string data, string hash);
    }
}