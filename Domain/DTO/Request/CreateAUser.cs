namespace Domain.DTO.Request
{
    public class CreateAUser
    {
        public readonly string Login;
        public readonly string Password;
        public CreateAUser(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}