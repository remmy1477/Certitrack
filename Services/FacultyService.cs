using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepository;
        public FacultyService(IFacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }
        public async Task<IEnumerable<Faculty>> GetAllFacultyAsync()
        {
            return await _facultyRepository.GetAllFacultyAsync();
        }
        public async Task<Faculty> GetFacultyByIdAsync(long Id)
        {
            return await _facultyRepository.GetFacultyByIdAsync(Id);
        }
    }
  
}
