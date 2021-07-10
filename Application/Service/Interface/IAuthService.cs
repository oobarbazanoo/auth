using System.Threading.Tasks;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Service.Interface
{
    public interface IAuthService
    {
        Task<SignupResponse> Signup(SignupRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}