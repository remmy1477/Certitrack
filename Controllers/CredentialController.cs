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


        public CredentialController(IUserService userService, IRoleService roleService,
            ICertificateDetailService CDService, IIInstitutionService institutionService,
            IQualificationClassService qualificationClassService, IQualificationTypeService qualificationTypeService,
            IFacultyService faculyService, ISchoolService schoolService, GEmailService gemailService)
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
        }   

        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;
                IEnumerable<CertificateDetail> credentials;
                //var credentials = _CDService.GetAllCertificateDetailAsync().Result
                //    .Skip((pageNumber - 1) * pageSize)
                //    .Take(pageSize)
                //    .ToList();

                string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
                string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";

                if (string.IsNullOrEmpty(schoolId) || schoolId =="0")
                {
                     credentials = await _CDService.GetAllCertificateDetailAsync();
                }
                else 
                {
                     credentials = (await _CDService.GetAllCertificateDetailAsync()).Where(cr => cr.SchoolId == int.Parse(schoolId) && cr.Ispaid);
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
            catch (Exception ex) { 

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
                CreatedBy = "System",
                LastModifiedBy = "System"
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





    }
}
