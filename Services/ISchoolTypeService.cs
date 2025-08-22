using Certitrack.Models;

namespace Certitrack.Services
{
    public interface ISchoolTypeService
    {
        Task<IEnumerable<SchoolType>> GetAllSchoolTypesAsync();
        Task<SchoolType> GetSchoolTypeByIdAsync(long Id);

        Task<string> InsertSchoolTypeAsync(SchoolType schoolType);
    }
}
