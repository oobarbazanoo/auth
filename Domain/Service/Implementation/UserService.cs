using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Service.Interface;

namespace Domain.Service.Implementation
{
    public class UserService : IUserService
    {
        readonly IUserRepository UserRepo;
        readonly ITokenManager TokenManager;
        public UserService(IUserRepository userRepo, ITokenManager tokenManager)
        {
            UserRepo = userRepo;
            TokenManager = tokenManager;
        }

        public async Task<UserCreationResult> Create(CreateAUser request)
        {
            var validationResult = CommonValidation(request.Login, request.Password);
            if (validationResult != null)
            {
                return new UserCreationResult(false, validationResult);
            }

            if (await UserRepo.LoginExist(request.Login))
            {
                return new UserCreationResult(false, "The login is taken");
            }

            var (salt, hash) = TokenManager.CreatePasswordHash(request.Password);
            await UserRepo.Create(request.Login, salt, hash);

            var token = TokenManager.GenerateToken(request.Login);

            return new UserCreationResult(true, null, token);
        }

        public async Task<UserLoginResult> Login(LoginAUser request)
        {
            var validationResult = CommonValidation(request.Login, request.Password);
            if (validationResult != null)
            {
                return new UserLoginResult(false, validationResult);
            }

            var foundUserResult = await UserRepo.FindByLogin(request.Login);

            if (foundUserResult != null &&
                TokenManager.IsPasswordCorrect(request.Password, foundUserResult.Salt, foundUserResult.Hash))
            {
                var token = TokenManager.GenerateToken(request.Login); ;
                return new UserLoginResult(true, null, token);
            }

            return new UserLoginResult(false, "Either Login or Password is incorrect");
        }

        string CommonValidation(string login, string password)
        {
            if (login == null || login.Trim().Length == 0 ||
                password == null || password.Trim().Length == 0)
            {
                return "Login and password must be present";
            }

            if (10 < login.Length)
            {
                return "Max login length is 10";
            }

            if (password.Length < 10 || 20 < password.Length)
            {
                return "Password should be 10 - 20 characters long";
            }

            return null;
        }
    }
}