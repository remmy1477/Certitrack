using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IIInstitutionService
    {
        Task<IEnumerable<Institution>> GetAllInstitutionsAsync();
        Task<Institution> GetInstitutionByIdAsync(long Id);

        Task<string> InsertInstitutionAsync(Institution institution);
    }
}
