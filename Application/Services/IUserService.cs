using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Services
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(int userId);

        public Task<List<User>> GetAllUsersAsync();

        public Task<int> CreateUserAsync(string login, string password);

        public Task DeleteUserAsync(int userId);
    }
}