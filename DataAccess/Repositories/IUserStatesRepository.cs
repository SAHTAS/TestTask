using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserStatesRepository
    {
        public Task<int> GetActiveStateIdAsync();

        public Task<int> GetBlockedStateIdAsync();
    }
}