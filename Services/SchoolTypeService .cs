using Certitrack.Models;
using Certitrack.Repositories;

namespace Certitrack.Services
{
    public class SchoolTypeService : ISchoolTypeService
    {
        private readonly ISchoolTypeRepository _schoolTypeRepository;
        public SchoolTypeService(ISchoolTypeRepository schoolTypeRepository)
        {
            _schoolTypeRepository = schoolTypeRepository;
        }

        public async Task<IEnumerable<SchoolType>> GetAllSchoolTypesAsync()
        {
            return await _schoolTypeRepository.GetAllSchoolTypesAsync();
        }

        public async Task<SchoolType> GetSchoolTypeByIdAsync(long Id)
        {
            return await _schoolTypeRepository.GetSchoolTypeByIdAsync(Id);  
        }

        public async Task<string> InsertSchoolTypeAsync(SchoolType schoolType)
        {
            try 
            {
                await _schoolTypeRepository.AddAsync(schoolType);
                return "School Type Detail Inserted";
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error inserting school type");
                return "error";
            }   
        }
        //public async Task<IEnumerable<School>> GetAllSchoolTypsAsync()
        //{
        //    return await _schoolTypeRepository.GetAllSchoolTypesAsync();
        //}

        //public async Task<School> GetSchoolByIdAsync(long Id)
        //{
        //    return await _schoolRepository.GetSchoolByIdAsync(Id);  
        //}

        //public async Task<string> InsertSchoolAsync(School school)
        //{
        //    try
        //    {
        //        await _schoolRepository.AddAsync(school);
        //        return "School Detail Inserted";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Optionally log the exception: _logger.LogError(ex, "Error inserting credential detail");
        //        return "error";
        //    }
        //}
    }
}
