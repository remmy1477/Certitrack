using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async  Task AddUserAsync(User user)
        {

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }

        public Task DeleteUserAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();  
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return user;
        }

        public async Task<User> GetByIdAsync(long id)
        {
            var user = await _context.Users.FindAsync(id);  
            return user;
        }

        public async  Task<User> GetByUsernameAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task UpdateUserAsync(User user)
        {
           _context.Users.Update(user);
           await _context.SaveChangesAsync();
        }
    }
}
