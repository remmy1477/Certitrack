using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        public SchoolService(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }
        public async Task<IEnumerable<School>> GetAllSchoolsAsync()
        {
            return await _schoolRepository.GetAllSchoolsAsync();
        }

        public async Task<School> GetSchoolByIdAsync(long Id)
        {
            return await _schoolRepository.GetSchoolByIdAsync(Id);  
        }

        public async Task<string> InsertSchoolAsync(School school)
        {
            try
            {
                await _schoolRepository.AddAsync(school);
                return "School Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
                return "error";
            }
        }
    }
}
