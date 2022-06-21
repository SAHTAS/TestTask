using System.Threading.Tasks;

namespace API.Services
{
    public interface ILoginService
    {
        public Task<string> GenerateTokenAsync(string login, string password);
    }
}