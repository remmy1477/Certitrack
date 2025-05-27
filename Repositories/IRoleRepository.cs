using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IRoleRepository
    {
       Task<IEnumerable<Role>> GetAllRolesAsync();  
       Task<Role> GetRoleByIdAsync(long Id);
    }
}
