using Certitrack.Models;

namespace Certitrack.Services
{
    public interface IInstitutionTypeService
    {
        Task<IEnumerable<InstitutionType>> GetAllInstitutionTypesAsync();
        Task<InstitutionType> GetInstitutionTypeByIdAsync(long Id);
    }
}
