using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserGroupsRepository
    {
        public Task<int> GetUserGroupIdAsync();
    }
}