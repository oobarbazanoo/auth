using System.Threading.Tasks;
using Domain.DTO.Response;
using Domain.Service.Interface;
using Infrastructure.Data.Configuration;
using Infrastructure.Data.Models.User;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly AuthDbContext Context;

        public UserRepository(AuthDbContext context)
        {
            Context = context;
        }

        public async Task Create(string login, byte[] salt, byte[] hash)
        {
            var newUser = new User
            {
                Login = login,
                Salt = salt,
                Hash = hash
            };

            await Context.Users.AddAsync(newUser);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> LoginExist(string login)
        {
            return await Context.Users
                .AsNoTracking()
                .CountAsync(u => u.Login == login) != 0;
        }

        public async Task<FoundUserResult> FindByLogin(string login)
        {
            var user = await Context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
            {
                return null;
            }

            return user.Adapt<FoundUserResult>();
        }
    }
}