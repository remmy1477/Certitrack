using Certitrack.Models;
using Certitrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Certitrack.Controllers
{
    [Route("Lookup")]
    public class LookupController : Controller
    {
        private readonly IIInstitutionService _iIInstitutionService;
        private readonly ISchoolService _schoolService;
        private readonly IFacultyService _facultyService;
        private readonly IDepartmentService _departmentyService;
        private readonly IQualificationTypeService _qualificationTypeService;
        private readonly IQualificationClassService _qualificationClassService;

        public LookupController(IIInstitutionService iIInstitutionService, ISchoolService schoolService, IFacultyService facultyService, IDepartmentService departmentyService,
            IQualificationTypeService qualificationTypeService, IQualificationClassService qualificationClassService)
        {
            _iIInstitutionService = iIInstitutionService;
            _schoolService = schoolService;
            _facultyService = facultyService;
            _departmentyService = departmentyService;
            _qualificationTypeService = qualificationTypeService;
            _qualificationClassService = qualificationClassService;


        }   

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateInstitution")]
        public async  Task<IActionResult> CreateInstitution(string name, int institutionTypeId)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(name) || institutionTypeId <= 0)
                    return BadRequest("Invalid data");

                var institution = new Institution
                {

                    Name = name,
                    InstitutionTypeId = institutionTypeId,
                    DateVerified = DateOnly.FromDateTime(DateTime.Now),
                    EmailDomain = string.Empty,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await _iIInstitutionService.InsertInstitutionAsync(institution);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the institution.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = institution.Id, name = institution.Name });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        
        [HttpPost("CreateSchool")]
        public async Task<IActionResult> CreateSchool(string name ,int schoolTypeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || schoolTypeId <= 0)
                    return BadRequest("Invalid data");

                var school = new School
                {

                    Name = name,
                    Address = "NA",
                    PhoneNumber = "NA",
                    Email = "NA",
                    SchoolTypeId = schoolTypeId,
                    Country = "NA",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await _schoolService.InsertSchoolAsync(school);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the School.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = school.Id, name = school.Name });
               // return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("CreateFaculty")]
        public async Task<IActionResult> CreateFaculty(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Invalid data");

                var faculty = new Faculty
                {

                    Name = name,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await _facultyService.AddAsync(faculty);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the Faculty.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = faculty.Id, name = faculty.Name });
                // return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Invalid data");

                var department = new Department
                {

                    Name = name,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await _departmentyService.AddAsync(department);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the Faculty.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = department.Id, name = department.Name });
                // return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("CreateQualificationType")]
        public async Task<IActionResult> CreateQualificationType(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Invalid data");

                var qualificationType = new QualificationType
                {

                    Name = name,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await _qualificationTypeService.AddAsync(qualificationType);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the Faculty.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = qualificationType.Id, name = qualificationType.Name });
                // return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("CreateQualificationClass")]
        public async Task<IActionResult> CreateQualificationClass(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Invalid data");

                var qualificationClass = new QualificationClass
                {

                    Name = name,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    CreatedBy = HttpContext.Session.GetString("UserEmail"),
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail")
                };

                var msg = await  _qualificationClassService.AddAsync(qualificationClass);
                if (msg == "error")
                    return StatusCode(500, "An error occurred while inserting the Faculty.");

                //_context.Institutions.Add(institution);
                //_context.SaveChanges();

                return Json(new { id = qualificationClass.Id, name = qualificationClass.Name });
                // return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
