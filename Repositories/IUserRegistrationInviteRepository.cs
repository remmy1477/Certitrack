using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IUserRegistrationInviteRepository
    {
        Task AddAsync(UserRegistrationInvite invite);
        Task<UserRegistrationInvite> GetByTokenAsync(string token);
        Task SaveChangesAsync();

        Task<IEnumerable<UserRegistrationInvite>> GetAllUserRegistrationInvitesAsync();

        Task UpdateAsync(UserRegistrationInvite invite);
    }
}
