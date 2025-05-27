using Certitrack.Models;
using Certitrack.Repositories;
using SendGrid.Helpers.Mail;

namespace Certitrack.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        //public async Task<User> AuthenticateAsync(string email, string password)
        //{
        //    var user = await _repository.GetByUsernameAsync(email);
        //    if (user == null) return null;

        //    var passwordHash = password;
        //    return user.PasswordHash == passwordHash ? user : null;
        //}

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = await _repository.GetByUsernameAsync(email);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return null;

            var passwordHash = password; // Consider hashing here if not already hashed

            //return user.PasswordHash == passwordHash ? user : null;
            return user;
        }

        public Task DeleteUserAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task RegisterUserAsync(User user)
        {
            var existingUser = await _repository.GetByUsernameAsync(user.Email);
            if (existingUser != null)
                throw new Exception("Username already taken.");

            //var newUser = new User
            //{
            //    Username = username,
            //    PasswordHash = ComputeHash(password)
            //};

            await _repository.AddUserAsync(user);
            

        }

        public async Task<string> UpdateUserAsync(User user)
        {
            await _repository.UpdateUserAsync(user);
            return "Record Updated Successfully";
        }

        
    }
}
