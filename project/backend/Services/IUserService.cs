using System.Threading.Tasks;
using MyBankApp.Models;

namespace MyBankApp.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Register(string username, string password);
        Task<User> GetByIdAsync(int id);
        Task UpdateUserAsync(User user);
    }
}
