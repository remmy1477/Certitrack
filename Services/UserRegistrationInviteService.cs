using Certitrack.Models;
using Certitrack.Repositories;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Certitrack.Services
{
    public class UserRegistrationInviteService : IUserRegistrationInviteService
    {
        private readonly IUserRegistrationInviteRepository _repository;

        public UserRegistrationInviteService(IUserRegistrationInviteRepository repository)
        {
            _repository = repository;
        }   
        public async Task<string> CreateInviteAsync(UserRegistrationInvite userRegistrationInvite)
        {
            await _repository.AddAsync(userRegistrationInvite);
            return userRegistrationInvite.Token;
        }

        public async Task<IEnumerable<UserRegistrationInvite>> GetAlllUserRegistrationInvitesAsync()
        {
            return await _repository.GetAllUserRegistrationInvitesAsync();
        }

        public async Task<string> UpdateInviteAsync(UserRegistrationInvite userRegistrationInvite)
        {
            await _repository.UpdateAsync(userRegistrationInvite);
            return "Record Updated Successfully";
        }

        public async Task<UserRegistrationInvite> ValidateTokenAsync(string token)
        {
            return await _repository.GetByTokenAsync(token);
        }


    }
}
