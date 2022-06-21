using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DataAccess.Repositories
{
    public interface IUsersRepository
    {
        public Task<User> GetByLoginAsync(string login);

        public Task<User> GetByLoginIncludeBlockedAsync(string login);

        public Task<bool> DoesLoginExistsAsync(string login);

        public Task<User> GetAsync(int userId);

        public Task<List<User>> GetAllAsync();

        public Task CreateOrUpdateAsync(User user);

        public Task DeleteAsync(int userId, int blockedStateId, DateTime? blockedDate);
    }
}