namespace Domain.DTO.Response
{
    public class UserCreationResult
    {
        public readonly bool WasCreated;
        public readonly string Message;
        public readonly string Token;

        public UserCreationResult(bool wasCreated, string message, string token = null)
        {
            WasCreated = wasCreated;
            Message = message;
            Token = token;
        }
    }
}
}
