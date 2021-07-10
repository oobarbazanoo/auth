using Application.DTO.Request;
using Application.DTO.Response;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Infrastructure.Data.Models.User;
using Mapster;

namespace Application.Mappings
{
    public class Mappings
    {
        public static void Register()
        {
            TypeAdapterConfig<SignupRequest, CreateAUser>.NewConfig()
                .MapWith(src => new CreateAUser(src.Login, src.Password));

            TypeAdapterConfig<UserCreationResult, SignupResponse>.NewConfig()
                .MapWith(src => new SignupResponse { Success = src.WasCreated, Message = src.Message });

            TypeAdapterConfig<LoginRequest, LoginAUser>.NewConfig()
                .MapWith(src => new LoginAUser(src.Login, src.Password));

            TypeAdapterConfig<UserLoginResult, LoginResponse>.NewConfig()
                .MapWith(src => new LoginResponse { Success = src.WasLoggedIn, Message = src.Message });

            TypeAdapterConfig<User, FoundUserResult>.NewConfig()
                .MapWith(src => new FoundUserResult(src.Login, src.Salt, src.Hash));
        }
    }
}