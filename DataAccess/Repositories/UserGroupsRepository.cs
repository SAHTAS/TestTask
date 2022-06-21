using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserGroupsRepository : IUserGroupsRepository
    {
        private readonly IDataContext context;

        public UserGroupsRepository(IDataContext context)
        {
            this.context = context;
        }
        
        public async Task<int> GetUserGroupIdAsync()
        {
            return await context.UserGroups
                .AsNoTracking()
                .Where(x => x.Code == UserGroupCode.User)
                .Select(x => x.UserGroupId)
                .FirstOrDefaultAsync();
        }
    }
}