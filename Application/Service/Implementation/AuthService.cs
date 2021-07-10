using System.Threading.Tasks;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Service.Interface;
using Domain.DTO.Request;
using Domain.Service.Interface;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.Service.Implementation
{
    public class AuthService : IAuthService
    {
        readonly ILogger<AuthService> Logger;
        readonly IUserService UserService;

        readonly ITokenManager TokenManager;

        public AuthService(
            ILogger<AuthService> logger,
            IUserService userService,
            ITokenManager tokenManager
        )
        {
            Logger = logger;
            UserService = userService;
            TokenManager = tokenManager;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var loginAUserRequest = request.Adapt<LoginAUser>();
            var userLoginResult = await UserService.Login(loginAUserRequest);
            var loginResponse = userLoginResult.Adapt<LoginResponse>();
            if (loginResponse.Success)
            {
                loginResponse.Token = TokenManager.GenerateToken(request.Login);
            }
            return loginResponse;
        }

        public async Task<SignupResponse> Signup(SignupRequest request)
        {
            var createAUserRequest = request.Adapt<CreateAUser>();
            var userCreationResult = await UserService.Create(createAUserRequest);
            var signupResponse = userCreationResult.Adapt<SignupResponse>();
            if (signupResponse.Success)
            {
                signupResponse.Token = TokenManager.GenerateToken(request.Login);
            }
            return signupResponse;
        }

    }
}