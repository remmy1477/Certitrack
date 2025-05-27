using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task RegisterUserAsync(User user);
        Task<User> GetUserByIdAsync(long id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<string> UpdateUserAsync(User user);
        Task DeleteUserAsync(long id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
