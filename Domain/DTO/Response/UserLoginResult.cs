namespace Domain.DTO.Response
{
    public class UserLoginResult
    {
        public readonly bool WasLoggedIn;
        public readonly string Message;
        public readonly string Token;

        public UserLoginResult(bool wasLoggedIn, string message, string token = null)
        {
            WasLoggedIn = wasLoggedIn;
            Message = message;
            Token = token;
        }
    }
}