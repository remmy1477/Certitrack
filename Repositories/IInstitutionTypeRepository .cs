using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface IInstitutionTypeRepository
    {
       Task<IEnumerable<InstitutionType>> GetAllIInstitutionTypeAsync();  
       Task<InstitutionType> GetIInstitutionTypeByIdAsync(long Id);
    }
}
