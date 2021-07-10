namespace Domain.DTO.Response
{
    public class FoundUserResult
    {
        public readonly string Login;
        public readonly byte[] Salt;
        public readonly byte[] Hash;
        public FoundUserResult(string login, byte[] salt, byte[] hash)
        {
            Login = login;
            Salt = salt;
            Hash = hash;
        }
    }
}