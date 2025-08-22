using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IInstitutionRepository
    {
        Task<IEnumerable<Institution>> GetAllInstitutionAsync();

        Task<Institution> GetInstitutionByIdAsync(long Id);

        Task AddAsync(Institution institution);
    }
}
