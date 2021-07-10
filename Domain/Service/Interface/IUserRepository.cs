using System.Threading.Tasks;
using Domain.DTO.Response;

namespace Domain.Service.Interface
{
    public interface IUserRepository
    {
        Task<bool> LoginExist(string login);
        Task Create(string login, byte[] salt, byte[] hash);
        Task<FoundUserResult> FindByLogin(string login);
    }
}