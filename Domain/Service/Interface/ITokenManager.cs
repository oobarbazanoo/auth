namespace Domain.Service.Interface
{
    public interface ITokenManager
    {
        (byte[] salt, byte[] hash) CreatePasswordHash(string password);
        string GenerateToken(string login);
        bool IsPasswordCorrect(string password, byte[] salt, byte[] hash);
    }
}