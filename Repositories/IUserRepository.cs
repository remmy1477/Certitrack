using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long id);
        Task<User> GetByIdAsync(long id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetByEmailAsync(string email);

    }
}
