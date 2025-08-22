using Certitrack.Models;

namespace Certitrack.Repositories
{
    public interface ISchoolTypeRepository
    {
        Task<IEnumerable<SchoolType>> GetAllSchoolTypesAsync();
        Task<SchoolType> GetSchoolTypeByIdAsync(long Id);

        Task AddAsync(SchoolType schoolType);
    }
}
