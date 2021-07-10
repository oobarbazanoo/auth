namespace Domain.DTO.Request
{
    public class LoginAUser
    {
        public readonly string Login;
        public readonly string Password;
        public LoginAUser(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}