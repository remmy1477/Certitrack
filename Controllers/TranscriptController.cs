using Certitrack.Models;
using Certitrack.Services;
using Certitrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using X.PagedList.Extensions;

namespace Certitrack.Controllers
{
    public class TranscriptController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly IIInstitutionService _institutionService;
        private readonly ITranscriptRequestService _transcriptRequestService;
        private readonly IQualificationClassService _qualificationClassService;
        private readonly IQualificationTypeService _qualificationTypeService;
        private readonly IFacultyService _faculyService;
        private readonly GEmailService _gemailService;

        public TranscriptController(ISchoolService schoolService, IIInstitutionService institutionService,
            ITranscriptRequestService transcriptRequestService, IQualificationClassService qualificationClassService,
            IQualificationTypeService qualificationTypeService, IFacultyService faculyService, GEmailService gemailService)
        {
            _schoolService = schoolService;
            _institutionService = institutionService;
            _transcriptRequestService = transcriptRequestService;
            _qualificationClassService = qualificationClassService;
            _qualificationTypeService = qualificationTypeService;
            _faculyService = faculyService;
            _gemailService = gemailService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;
                IEnumerable<TranscriptRequest> transcriptRequests;

                

                string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
                string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";

                if (string.IsNullOrEmpty(schoolId) || schoolId == "0")
                {
                    transcriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
                }
                else
                {
                    transcriptRequests = (await _transcriptRequestService.GetAllTranscriptRequestAsync()).Where(cr => cr.SchoolId == int.Parse(schoolId) && cr.IsPaid);
                }

                var transcriptRequestsList = new List<TranscriptRequestIndexVM>();
                if (transcriptRequests == null || !transcriptRequests.Any())
                {
                    TempData["Message"] = "No Transcript Request found.";
                    //var pagedList = creditialsList
                    return View(transcriptRequestsList.ToList().ToPagedList());
                }
                else
                {
                    foreach (var transcriptRequest in transcriptRequests)
                    {
                        var school = await _schoolService.GetSchoolByIdAsync(transcriptRequest.SchoolId);
                        var institution = await _institutionService.GetInstitutionByIdAsync(transcriptRequest.InstitutionId);
                        var faculty = await _faculyService.GetFacultyByIdAsync(transcriptRequest.FacultyId);
                        //var qualificationType = await _qualificationTypeService.GetQualificationTypeByIdAsync(credential.DegreeId);
                       // var qualificationClass = await _qualificationClassService.GetQualificationClassByIdAsync(credential.DegreeClassId);
                        transcriptRequestsList.Add(new TranscriptRequestIndexVM
                        {
                            Id = transcriptRequest.Id,
                            FirstName = transcriptRequest.FirstName,
                            LastName = transcriptRequest.LastName,
                            MiddleName = transcriptRequest.MiddleName,
                            MatricNumber = transcriptRequest.MatricNumber,
                            SchoolId = transcriptRequest.SchoolId,
                            SchoolName = school.Name,
                            DestinationEmail = transcriptRequest.DestinationEmail,  
                            FacultyId = transcriptRequest.FacultyId,
                            FacultyName = faculty.Name,
                            Department = transcriptRequest.Department,
                            YearOfGraduation = transcriptRequest.YearOfGraduation,
                            InstitutionId = transcriptRequest.InstitutionId,
                            InstitutionName = institution.Name,
                            TranscriptFilePath = transcriptRequest.TranscriptFilePath,
                            Status = transcriptRequest.Status,
                            NonVerificationReason = transcriptRequest.NonVerificationReason,
                            IsPaid = transcriptRequest.IsPaid,  

                        });
                    }
                }

                var pagedList = transcriptRequestsList.OrderBy(u => u.InstitutionId).ToList().ToPagedList(pageNumber, pageSize);




                return View(pagedList);
            } catch (Exception ex) 
            {
                return View(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var schools = await _schoolService.GetAllSchoolsAsync();
                var institutions = await _institutionService.GetAllInstitutionsAsync();
                var faculties = await _faculyService.GetAllFacultyAsync();

                ViewBag.Schools = schools;
                ViewBag.Institutions = institutions;
                ViewBag.Faculties = faculties;


                return View();
            }
            catch (Exception ex) {
                return View(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(TranscriptRequestVM transcriptRequestVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var transcriptRequest = new TranscriptRequest
                {
                    FirstName = transcriptRequestVM.FirstName,
                    LastName = transcriptRequestVM.LastName,
                    MiddleName = transcriptRequestVM.MiddleName,
                    MatricNumber = transcriptRequestVM.MatricNumber,
                    InstitutionId = transcriptRequestVM.InstitutionId,
                    SchoolId = transcriptRequestVM.SchoolId,
                    FacultyId = transcriptRequestVM.FacultyId,
                    Department = transcriptRequestVM.Department,
                    YearOfGraduation = transcriptRequestVM.YearOfGraduation,
                    DestinationEmail = transcriptRequestVM.DestinationEmail,
                    Status = "Pending",
                    TranscriptFilePath ="",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "System",
                    LastModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "System",
                    NonVerificationReason ="",
                    NonApprovalReason = "",
                    IsPaid = false,

                };
                var msg = await _transcriptRequestService.InsertTranscriptRequestAsync(transcriptRequest);

                TempData["Message"] = msg;


                return RedirectToAction("Index", "Transcript");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyTranscriptRequest(long Id, bool recordExists,string? reason)
        {
            var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(Id);

            if (request == null)
            {
                return NotFound(new { success = false, message = "Transcript request not found." });
            }
            if (recordExists)
            {
                request.Status = "Verified";
                request.NonVerificationReason = ""; // Clear any previous non-verification reason
            }
            else
            {
                request.Status = "Not Verified";
                request.NonVerificationReason = reason ?? reason; // Use provided reason or default message
            }
            request.Updated = DateTime.UtcNow;

            await _transcriptRequestService.UpdateTranscriptRequestAsync(request);  
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAndSendTranscript(IFormFile transcriptFile, string requestId)
        {
            var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(long.Parse(requestId));
            if (request == null || request.Status != "Verified")
                return BadRequest("Request not ready or does not exist.");

            if (transcriptFile == null || transcriptFile.Length == 0)
                return BadRequest("Transcript file is required.");

            // Save the file
            var filePath = Path.Combine("wwwroot", "transcripts", transcriptFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await transcriptFile.CopyToAsync(stream);
            }

            request.Status = $"Confirmed by Senior Officer, and transcript sent to {request.DestinationEmail}";
            request.TranscriptFilePath = filePath;
            await _transcriptRequestService.UpdateTranscriptRequestAsync(request);

            
            var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}";
            var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            string subject = $"CertiTrack – Request For Transcript – - {fullName}";
            string body = $@"
                        <p>Dear Sir/Madam,</p>
                        <p>We are writing to confirm  that the academic transcript <br> in respect of {fullName} inclusive of the course details, grades, and duration of study,
                        <p>have been confirmed and accordingly forwarded to the institution provided by you.
                        <br/>
                        <br/>
                        <p>Kind regards,
                        <p>[Registrar’s Full Name]
                        <p>Registrar
                        <p>{school.Name}</p>
                        <p>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</p>";

            try
            {
                await _gemailService.SendEmailWithAttachmentAsync(request.DestinationEmail, subject, body, filePath);
                ViewBag.Message = "Transcript Request Mail sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to send Transcript Request email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }

            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RejectTranscriptRequest(long Id, string reason)
        {
            var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(Id);
            if (request == null) return NotFound();

            request.Status = "Request not ready or does not exist.";
            await _transcriptRequestService.UpdateTranscriptRequestAsync(request);

            var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}";
            var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            string subject = $"CertiTrack – Confirmation of Credential Verification - {fullName}";
            string body = $@"
                        <p>Dear Sir/Madam,</p>
                        <p>We regret to inform you that the transcript record for {fullName} could not be found.\n\nReason: {reason}
                       
                        <br/>
                        <br/>
                        <p>Kind regards,
                        <p>[Registrar’s Full Name]
                        <p>Registrar
                        <p>{school.Name}</p>
                        <p>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</p>";

            try
            {
                await _gemailService.SendEmailAsync(request.DestinationEmail, subject, body);
                ViewBag.Message = "Transcript Request  Mail sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed  email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }

            return Ok(new { success = true });
        }


    }
}
