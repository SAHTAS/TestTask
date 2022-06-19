using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface IUserStatesRepository
    {
        public Task<int> GetActiveStateIdAsync();

        public Task<int> GetBlockedStateIdAsync();
    }
    
    public class UserStatesRepository : IUserStatesRepository
    {
        private readonly IDataContext context;

        public UserStatesRepository(IDataContext context)
        {
            this.context = context;
        }
        
        public async Task<int> GetActiveStateIdAsync()
        {
            return await GetStateIdAsync(UserStateCode.Active);
        }
        
        public async Task<int> GetBlockedStateIdAsync()
        {
            return await GetStateIdAsync(UserStateCode.Blocked);
        }

        private async Task<int> GetStateIdAsync(UserStateCode code)
        {
            return await context.UserStates
                .AsNoTracking()
                .Where(x => x.Code == code)
                .Select(x => x.UserStateId)
                .FirstOrDefaultAsync();
        }
    }
}