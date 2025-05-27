using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(long Id);
    }
}
