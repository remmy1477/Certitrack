using Certitrack.Data;
using Certitrack.Models;
using Certitrack.Services;
using Certitrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using X.PagedList.Extensions;

namespace Certitrack.Controllers
{
    public class CredentialController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICertificateDetailService _CDService;
        private readonly ISchoolService _schoolService;
        private readonly IIInstitutionService _institutionService;
        private readonly IQualificationClassService _qualificationClassService;
        private readonly IQualificationTypeService _qualificationTypeService;
        private readonly IFacultyService _faculyService;
        private readonly GEmailService _gemailService;
        private readonly AppDbContext _context;


        public CredentialController(IUserService userService, IRoleService roleService,
            ICertificateDetailService CDService, IIInstitutionService institutionService,
            IQualificationClassService qualificationClassService, IQualificationTypeService qualificationTypeService,
            IFacultyService faculyService, ISchoolService schoolService, GEmailService gemailService, AppDbContext context)
        {
            _userService = userService;
            _roleService = roleService;
            _CDService = CDService;
            _institutionService = institutionService;
            _qualificationClassService = qualificationClassService;
            _qualificationTypeService = qualificationTypeService;
            _faculyService = faculyService;
            _schoolService = schoolService;
            _gemailService = gemailService;
            _context = context;
        }

        //public async Task<IActionResult> Index(int? page)
        //{
        //    try
        //    {
        //        int pageSize = 10;
        //        int pageNumber = page ?? 1;

        //        var allCredentials = await _CDService.GetAllCertificateDetailAsync();

        //        string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
        //        string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
        //        string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
        //        string roleName = HttpContext?.Session?.GetString("UserRoleName");

        //        IEnumerable<CertificateDetail> credentials = Enumerable.Empty<CertificateDetail>();

        //        if (roleName == "SuperAdmin" || roleName == "AdminUser")
        //        {
        //            credentials = allCredentials;
        //        }
        //        else if (!string.IsNullOrEmpty(schoolId) && schoolId != "0")
        //        {
        //            credentials = allCredentials.Where(cr => cr.SchoolId == long.Parse(schoolId));
        //        }
        //        else if (!string.IsNullOrEmpty(institutionId) && institutionId != "0")
        //        {
        //            credentials = allCredentials.Where(cr => cr.InstitutionId == long.Parse(institutionId));
        //        }

        //        var creditialsList = new List<CredentialDetailIndexVM>();

        //        if (credentials == null || !credentials.Any())
        //        {
        //            TempData["Message"] = "No Credential Detail found.";
        //            return View(creditialsList.ToPagedList(pageNumber, pageSize));
        //        }
        //        else
        //        {
        //            foreach (var credential in credentials)
        //            {
        //                var schoolTask = _schoolService.GetSchoolByIdAsync(credential.SchoolId);
        //                var institutionTask = _institutionService.GetInstitutionByIdAsync(credential.InstitutionId);
        //                var facultyTask = _faculyService.GetFacultyByIdAsync(credential.FacultyId);
        //                var qualificationTypeTask = _qualificationTypeService.GetQualificationTypeByIdAsync(credential.DegreeId);
        //                var qualificationClassTask = _qualificationClassService.GetQualificationClassByIdAsync(credential.DegreeClassId);

        //                await Task.WhenAll(schoolTask, institutionTask, facultyTask, qualificationTypeTask, qualificationClassTask);

        //                var school = await schoolTask;
        //                var institution = await institutionTask;
        //                var faculty = await facultyTask;
        //                var qualificationType = await qualificationTypeTask;
        //                var qualificationClass = await qualificationClassTask;

        //                creditialsList.Add(new CredentialDetailIndexVM
        //                {
        //                    Id = credential.Id,
        //                    HolderFirstName = credential.HolderFirstName,
        //                    HolderLastName = credential.HolderLastName,
        //                    HolderMiddleName = credential.HolderMiddleName,
        //                    HolderEmail = credential.HolderEmail,
        //                    HolderPhoneNumber = credential.HolderPhoneNumber,
        //                    HolderAddress = credential.HolderAddress,
        //                    CertificateNo = credential.CertificateNo,
        //                    Degree = credential.DegreeId,
        //                    DegreeNm = qualificationType?.Name,
        //                    DegreeClass = credential.DegreeClassId,
        //                    DegreeClassNm = qualificationClass?.Name,
        //                    Faculty = credential.FacultyId,
        //                    FacultyNm = faculty?.Name,
        //                    Department = credential.Department,
        //                    YearOfGraduation = credential.YearOfGraduation,
        //                    SchoolId = credential.SchoolId,
        //                    SchoolNm = school?.Name,
        //                    InstitutionNm = institution?.Name,
        //                    IsVerified = credential.IsVerified,
        //                    Status = credential.Status,
        //                    InstitutionId = credential.InstitutionId,
        //                    Ispaid = credential.Ispaid
        //                });
        //            }
        //        }

        //        var pagedList = creditialsList.OrderBy(u => u.InstitutionId).ToPagedList(pageNumber, pageSize);

        //        return View(pagedList);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Consider logging the exception
        //        return View("Error", ex);
        //    }
        //}


        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;

                var allCredentials = await _CDService.GetAllCertificateDetailAsync();

                //IEnumerable<CertificateDetail> credentials;


                string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
                string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
                string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
                string roleName = HttpContext?.Session?.GetString("UserRoleName");
                var UserEmail = HttpContext?.Session?.GetString("UserEmail");

                IEnumerable<CertificateDetail> credentials = Enumerable.Empty<CertificateDetail>();

                if (roleName == "SuperAdmin" || roleName == "AdminUser")
                {
                    credentials = allCredentials;

                }
                else if(roleName == "Student" || roleName == "Agent")
                { 
                    credentials = allCredentials.Where(cr => cr.CreatedBy == UserEmail);
                }
                else if (schoolId != "0")
                {
                    //var schCedentials = await _CDService.GetAllCertificateDetailAsync();
                    credentials = allCredentials.Where(cr => cr.SchoolId == long.Parse(schoolId));
                }
                else if (institutionId != "0")
                {
                    //var instCredentials = await _CDService.GetAllCertificateDetailAsync();
                    credentials = allCredentials.Where(cr => cr.InstitutionId == long.Parse(institutionId));
                }

                var creditialsList = new List<CredentialDetailIndexVM>();

                if (credentials == null || !credentials.Any())
                {
                    TempData["Message"] = "No Credential Detail found.";
                    //var pagedList = creditialsList
                    return View(creditialsList.ToList().ToPagedList());
                }
                else
                {
                    foreach (var credential in credentials)
                    {
                        var school = await _schoolService.GetSchoolByIdAsync(credential.SchoolId);
                        var institution = await _institutionService.GetInstitutionByIdAsync(credential.InstitutionId);
                        var faculty = await _faculyService.GetFacultyByIdAsync(credential.FacultyId);
                        var qualificationType = await _qualificationTypeService.GetQualificationTypeByIdAsync(credential.DegreeId);
                        var qualificationClass = await _qualificationClassService.GetQualificationClassByIdAsync(credential.DegreeClassId);
                        creditialsList.Add(new CredentialDetailIndexVM
                        {
                            Id = credential.Id,
                            HolderFirstName = credential.HolderFirstName,
                            HolderLastName = credential.HolderLastName,
                            HolderMiddleName = credential.HolderMiddleName,
                            HolderEmail = credential.HolderEmail,
                            HolderPhoneNumber = credential.HolderPhoneNumber,
                            HolderAddress = credential.HolderAddress,
                            CertificateNo = credential.CertificateNo,
                            Degree = credential.DegreeId,
                            DegreeNm = qualificationType.Name,
                            DegreeClass = credential.DegreeClassId,
                            DegreeClassNm = qualificationClass.Name,
                            Faculty = credential.FacultyId,
                            FacultyNm = faculty.Name,
                            Department = credential.Department,
                            YearOfGraduation = credential.YearOfGraduation,
                            SchoolId = credential.SchoolId,
                            SchoolNm = school.Name,
                            InstitutionNm = institution.Name,
                            IsVerified = credential.IsVerified,
                            Status = credential.Status,
                            InstitutionId = credential.InstitutionId,
                            Ispaid = credential.Ispaid
                        });
                    }
                }

                var pagedList = creditialsList.OrderBy(u => u.InstitutionId).ToList().ToPagedList(pageNumber, pageSize);




                return View(pagedList);
            }
            catch (Exception ex)
            {

                return View(ex);
            }

        }

        public async Task<IActionResult> Create()
        {
            var institutions = await _institutionService.GetAllInstitutionsAsync();
            ViewBag.institutions = institutions;
            var qualificationTypes = await _qualificationTypeService.GetAllQualificationTypeAsync();
            ViewBag.qualificationTypes = qualificationTypes;
            var qualificationClasses = await _qualificationClassService.GetAllQualificationClassAsync();
            ViewBag.qualificationClasses = qualificationClasses;
            var faculties = await _faculyService.GetAllFacultyAsync();
            ViewBag.faculties = faculties;
            var schools = await _schoolService.GetAllSchoolsAsync();
            ViewBag.schools = schools;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CredentialDetailVM credentialDetailVM)
        {
            if (!ModelState.IsValid)
                return View();

            var UserEmail = HttpContext?.Session?.GetString("UserEmail");

            var cred = new CertificateDetail
            {
                HolderFirstName = credentialDetailVM.HolderFirstName,
                HolderLastName = credentialDetailVM.HolderLastName,
                HolderMiddleName = credentialDetailVM.HolderMiddleName,
                HolderEmail = credentialDetailVM.HolderEmail,
                HolderPhoneNumber = credentialDetailVM.HolderPhoneNumber,
                HolderAddress = credentialDetailVM.HolderAddress,
                CertificateNo = credentialDetailVM.CertificateNo,
                DegreeId = credentialDetailVM.Degree,
                DegreeClassId = credentialDetailVM.DegreeClass,
                FacultyId = credentialDetailVM.Faculty,
                Department = credentialDetailVM.Department,
                YearOfGraduation = credentialDetailVM.YearOfGraduation,
                SchoolId = credentialDetailVM.SchoolId,
                InstitutionId = credentialDetailVM.InstitutionId,
                IsVerified = false,
                Ispaid = false, 
                Status = VerificationStatus.Pending.ToString(),
                Created = DateTime.Now,
                Updated = DateTime.Now,
                CreatedBy = UserEmail ?? "System",
                LastModifiedBy = UserEmail ?? "System"
            };

            var msg = await _CDService.InsertCredenttialDetailAsync(cred);

            TempData["Message"] = msg;


            return RedirectToAction("Index", "Credential");
        }

        //[HttpGet] 
        //public async Task<IActionResult> Verify()
        //{
        //    return View();  
        //}

        [HttpPost]
        public async Task<IActionResult> Verify(string Id)
        {
            var credential = await _CDService.GetCertificateDetailByIdAsync(long.Parse(Id));

            if (credential == null)
            {
                return Json(new { success = false, message = "Credential not found." });
            }
            credential.IsVerified = true;

            await _CDService.UpdateCredenttialDetailAsync(credential);
            // Your logic to mark as verified
            return Json(new { success = true, message = "Certificate verification submitted." });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAuthentic(string Id)
        {
            // Your logic to mark as authentic
            var credential = await _CDService.GetCertificateDetailByIdAsync(long.Parse(Id));

            if (credential == null)
            {
                return Json(new { success = false, message = "Credential not found." });
            }
            credential.Status = "Genuine";
            await _CDService.UpdateCredenttialDetailAsync(credential);
            var fullName = $"{credential.HolderFirstName} {credential.HolderMiddleName} {credential.HolderLastName}";
            var school = await _schoolService.GetSchoolByIdAsync(credential.SchoolId);
            string subject = $"CertiTrack – Confirmation of Credential Verification - {fullName}";
            string body = $@"
                        <p>Dear Sir/Madam,</p>
                        <p>We are writing to confirm that the academic credentials submitted in<br> respect of {fullName} have been verified and found to be<br> accurate.</p>
                        <p>The verification was completed by the issuing institution, and all details<br> provided align with the official academic records on file.
                        <br/>
                        <br/>
                        <p>Kind regards,
                        <p>[Registrar’s Full Name]
                        <p>Registrar
                        <p>{school.Name}</p>
                        <p>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</p>";

            try
            {
                await _gemailService.SendEmailAsync("remmyojediran@gmail.com", subject, body);
                ViewBag.Message = "Credential Verification Mail sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to send invitation email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }

            return Json(new { success = true, message = "Credential confirmed as authentic." });
        }

        [HttpPost]
        public async Task<IActionResult> RejectCredential(string Id)
        {
            // Your logic to reject
            var credential = await _CDService.GetCertificateDetailByIdAsync(long.Parse(Id));

            if (credential == null)
            {
                return Json(new { success = false, message = "Credential not found." });
            }
            credential.Status = "NotGenuine";
            await _CDService.UpdateCredenttialDetailAsync(credential);
            var fullName = $"{credential.HolderFirstName} {credential.HolderMiddleName} {credential.HolderLastName}";
            var school = await _schoolService.GetSchoolByIdAsync(credential.SchoolId);
            string subject = $"CertiTrack – Confirmation of Credential Verification - {fullName}";
            string body = $@"
                        <p>Dear Sir/Madam,</p>
                        <p>We are writing to confirm that the academic credentials submitted in<br> respect of {fullName} have been verified and found to be<br> inaccurate.</p>
                        <p>The verification was completed by the issuing institution, and all or some details<br> provided do not align with the official academic records on file.
                        <br/>
                        <br/>
                        <p>Kind regards,
                        <p>[Registrar’s Full Name]
                        <p>Registrar
                        <p>{school.Name}</p>
                        <p>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</p>";

            try
            {
                await _gemailService.SendEmailAsync("remmyojediran@gmail.com", subject, body);
                ViewBag.Message = "Credential Verification Mail sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to send invitation email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }

            return Json(new { success = true, message = "Credential marked as not authentic." });
        }

        [HttpGet]
        public async Task<IActionResult> CreateBatchCredential()
        {
            var institutions = await _institutionService.GetAllInstitutionsAsync();
            ViewBag.institutions = institutions;
            var qualificationTypes = await _qualificationTypeService.GetAllQualificationTypeAsync();
            ViewBag.qualificationTypes = qualificationTypes;
            var qualificationClasses = await _qualificationClassService.GetAllQualificationClassAsync();
            ViewBag.qualificationClasses = qualificationClasses;
            var faculties = await _faculyService.GetAllFacultyAsync();
            ViewBag.faculties = faculties;
            var schools = await _schoolService.GetAllSchoolsAsync();
            ViewBag.schools = schools;
            return View();
        }

       [HttpPost]
        public async Task<IActionResult> CreateBatchCredential([FromBody] CredentialBatchVM batchVM)
        {
            var resultMessage = await _CDService.BatchInsertAsync(batchVM.CredentialDetails);

            bool isSuccess = !resultMessage.StartsWith("Error");
            return Json(new { success = isSuccess, message = resultMessage });
        }




    }
}
