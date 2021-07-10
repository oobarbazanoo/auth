using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Service.Interface
{
    public interface IUserService
    {
        Task<UserCreationResult> Create(CreateAUser request);
        Task<UserLoginResult> Login(LoginAUser request);
    }
}