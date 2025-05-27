using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }
        public async Task<Role> GetRoleByIdAsync(long Id)
        {
            return await _roleRepository.GetRoleByIdAsync(Id);
        }
    }   
   
}
