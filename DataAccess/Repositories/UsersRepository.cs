using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface IUsersRepository
    {
        public Task<User> GetByLoginAsync(string login);

        public Task<bool> DoesLoginExistsAsync(string login);

        public Task<User> GetAsync(int userId);

        public Task<List<User>> GetAllAsync();

        public Task CreateAsync(User user);

        public Task DeleteAsync(int userId, int blockedStateId);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly IDataContext context;

        public UsersRepository(IDataContext context)
        {
            this.context = context;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            return await context.Users
                .AsNoTracking()
                .Include(x => x.UserGroup)
                .Where(x => x.Login == login && x.UserState.Code == UserStateCode.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DoesLoginExistsAsync(string login)
        {
            return await context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Login == login && x.UserState.Code == UserStateCode.Active);
        }

        public async Task<User> GetAsync(int userId)
        {
            return await context.Users
                .AsNoTracking()
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .Where(x => x.UserId == userId && x.UserState.Code == UserStateCode.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await context.Users
                .AsNoTracking()
                .Include(x => x.UserGroup)
                .Include(x => x.UserState)
                .Where(x => x.UserState.Code == UserStateCode.Active)
                .ToListAsync();
        }

        public async Task CreateAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId, int blockedStateId)
        {
            var user = await context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.UserStateId = blockedStateId;
                await context.SaveChangesAsync();
            }
        }
    }
}