using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class InstitutionTypeService : IInstitutionTypeService
    {
        private readonly IInstitutionTypeRepository _institutionTypeRepository;
        public InstitutionTypeService(IInstitutionTypeRepository institutionTypeRepository)
        {
            _institutionTypeRepository = institutionTypeRepository;
        }

        public async Task<IEnumerable<InstitutionType>> GetAllInstitutionTypesAsync()
        {
            return await _institutionTypeRepository.GetAllIInstitutionTypeAsync();
        }

        

        public async Task<InstitutionType> GetInstitutionTypeByIdAsync(long Id)
        {
            return await _institutionTypeRepository.GetIInstitutionTypeByIdAsync(Id);
        }

      
    }   
   
}
