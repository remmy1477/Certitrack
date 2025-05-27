using Certitrack.Data;
using Certitrack.Models;
using Microsoft.EntityFrameworkCore;

namespace Certitrack.Repositories
{
    public class UserRegistrationInviteRepository : IUserRegistrationInviteRepository
    {
        private readonly AppDbContext _context;

        public UserRegistrationInviteRepository(AppDbContext context)
        {
            _context = context;
        }   
        public async Task AddAsync(UserRegistrationInvite invite)
        {
            _context.UserRegistrationInvites.Add(invite);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRegistrationInvite>> GetAllUserRegistrationInvitesAsync()
        {
            return await _context.UserRegistrationInvites.ToListAsync();
        }

        public async Task<UserRegistrationInvite> GetByTokenAsync(string token)
        {
            return await _context.UserRegistrationInvites
             .FirstOrDefaultAsync(i => i.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserRegistrationInvite invite)
        {
            _context.UserRegistrationInvites.Update(invite);
            await _context.SaveChangesAsync();
        }
    }
}
