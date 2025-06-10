using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class QualificationClassService : IQualificationClassService
    {
        private readonly IQualificationClasssRepository _qualificationClassRepository;

        public QualificationClassService(IQualificationClasssRepository qualificationClassRepository)
        {
            _qualificationClassRepository = qualificationClassRepository;
        }

        public async Task<IEnumerable<QualificationClass>> GetAllQualificationClassAsync()
        {
            return await _qualificationClassRepository.GetAllQualificationClassAsync();
        }

        public async  Task<QualificationClass> GetQualificationClassByIdAsync(long Id)
        {
            return await _qualificationClassRepository.GetQualificationClassByIdAsync(Id);    
        }

        
    }
}
