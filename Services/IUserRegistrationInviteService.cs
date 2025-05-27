using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IUserRegistrationInviteService
    {
        Task<string> CreateInviteAsync(UserRegistrationInvite userRegistrationInvite);
        Task<UserRegistrationInvite> ValidateTokenAsync(string token);

        Task<IEnumerable<UserRegistrationInvite>> GetAlllUserRegistrationInvitesAsync();

        Task<string> UpdateInviteAsync(UserRegistrationInvite userRegistrationInvite);
    }
}
