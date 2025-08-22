using Certitrack.Data;
using Certitrack.Models;
using Certitrack.Services;
using Certitrack.Services.Certitrack.Services;
using Certitrack.Utility;
using Certitrack.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly AppDbContext _context;
        private readonly GEmailService _gemailService;
        private readonly IDepartmentService _departmentService;
        private readonly IInstitutionTypeService _institutionTypeService;
        private readonly ISchoolTypeService _schoolTypeService;

        public TranscriptController(ISchoolService schoolService, IIInstitutionService institutionService,
            ITranscriptRequestService transcriptRequestService, IQualificationClassService qualificationClassService,
            IQualificationTypeService qualificationTypeService, IFacultyService faculyService, GEmailService gemailService,
            IDepartmentService departmentService, IInstitutionTypeService institutionTypeService, ISchoolTypeService schoolTypeService, AppDbContext context)
        {
            _schoolService = schoolService;
            _institutionService = institutionService;
            _transcriptRequestService = transcriptRequestService;
            _qualificationClassService = qualificationClassService;
            _qualificationTypeService = qualificationTypeService;
            _faculyService = faculyService;
            _gemailService = gemailService;
            _departmentService = departmentService;
            _institutionTypeService = institutionTypeService;
            _schoolTypeService = schoolTypeService;
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;

                var alltranscripts = await _transcriptRequestService.GetAllTranscriptRequestAsync(); 
                           

                string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
                string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
                string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
                string roleName = HttpContext?.Session?.GetString("UserRoleName");
                var UserEmail = HttpContext?.Session?.GetString("UserEmail");

                IEnumerable<TranscriptRequest> transcriptRequests =  Enumerable.Empty<TranscriptRequest>();

                if (roleName == "SuperAdmin" || roleName == "AdminUser")
                {
                    transcriptRequests = alltranscripts;
                }
                else if(roleName == "Student" || roleName == "Agent")
                { 
                    transcriptRequests = alltranscripts.Where(tr => tr.CreatedBy == UserEmail);
                }    
                else if (schoolId != "0")
                {
                    transcriptRequests = alltranscripts.Where(tr => tr.SchoolId == long.Parse(schoolId));
                }
                else if (institutionId != "0")
                {
                    transcriptRequests = alltranscripts.Where(tr => tr.InstitutionId == long.Parse(institutionId));
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
                var departments = await _departmentService.GetAllDepartmentAsync();
                var institutionTypes = await _institutionTypeService.GetAllInstitutionTypesAsync();
                var schoolTypes = await _schoolTypeService.GetAllSchoolTypesAsync();

                ViewBag.Schools = schools;
                ViewBag.Institutions = institutions;
                ViewBag.Faculties = faculties;
                ViewBag.Departments = departments;
                ViewBag.InstitutionTypes = institutionTypes;
                ViewBag.SchoolTypes = schoolTypes;

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
                    StudentEmail = transcriptRequestVM.StudentEmail ?? string.Empty, // Optional, can be empty if not provided
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

        //[HttpPost]
        //public async Task<IActionResult> ApproveAndSendTranscript(IFormFile transcriptFile, string requestId)
        //{
        //    var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(long.Parse(requestId));
        //    if (request == null || request.Status != "Verified")
        //        return BadRequest("Request not ready or does not exist.");

        //    if (transcriptFile == null || transcriptFile.Length == 0)
        //        return BadRequest("Transcript file is required.");

        //    // Save the file
        //    var filePath = Path.Combine("wwwroot", "transcripts", transcriptFile.FileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await transcriptFile.CopyToAsync(stream);
        //    }

        //    request.Status = $"Confirmed by Senior Officer, and transcript sent to {request.DestinationEmail}";
        //    request.TranscriptFilePath = filePath;
        //    await _transcriptRequestService.UpdateTranscriptRequestAsync(request);


        //    var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}";
        //    var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
        //    string subject = $"CertiTrack – Request For Transcript – - {fullName}";
        //    string body = $@"
        //                <p>Dear Sir/Madam,</p>
        //                <p>We are writing to confirm  that the academic transcript <br> in respect of {fullName} inclusive of the course details, grades, and duration of study,
        //                <p>have been confirmed and accordingly forwarded to the institution provided by you.
        //                <br/>
        //                <br/>
        //                <p>Kind regards,

        //                <p>{school.Name}</p>
        //                <p>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</p>";

        //    try
        //    {
        //        await _gemailService.SendEmailWithAttachmentAsync(request.DestinationEmail, subject, body, filePath);
        //        ViewBag.Message = "Transcript Request Mail sent successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);//"Failed to send Transcript Request email. Please try again.")
        //        // Optionally log: _logger.LogError(ex, "Email sending failed.");
        //    }

        //    return Ok(new { success = true });
        //}

        //[HttpPost]
        //public async Task<IActionResult> ApproveAndSendTranscript(IFormFile transcriptFile, string requestId)
        //{
        //    var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(long.Parse(requestId));
        //    if (request == null || request.Status != "Verified")
        //        return BadRequest("Request not ready or does not exist.");

        //    if (transcriptFile == null || transcriptFile.Length == 0)
        //        return BadRequest("Transcript file is required.");

        //    var ext = Path.GetExtension(transcriptFile.FileName).ToLowerInvariant();
        //    var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        //    Directory.CreateDirectory(tempDir);

        //    var originalFilePath = Path.Combine(tempDir, transcriptFile.FileName);

        //    using (var stream = new FileStream(originalFilePath, FileMode.Create))
        //    {
        //        await transcriptFile.CopyToAsync(stream);
        //    }

        //    byte[] originalBytes = await System.IO.File.ReadAllBytesAsync(originalFilePath);
        //    byte[] officialPdf;
        //    string pdfFileName = Path.GetFileNameWithoutExtension(transcriptFile.FileName) + ".pdf";

        //    // Convert to PDF if needed
        //    if (ext == ".pdf")
        //    {
        //        officialPdf = originalBytes;
        //    }
        //    else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
        //    {
        //        officialPdf = ImageToPdfConverter.ConvertImageToPdf(originalBytes);
        //    }
        //    //else if (ext == ".doc" || ext == ".docx" || ext == ".xls" || ext == ".xlsx")
        //    //{
        //    //    officialPdf = OfficeToPdfConverter.ConvertToPdf(originalFilePath);
        //    //}
        //    else
        //    {
        //        return BadRequest("Unsupported file type. Only PDF, Word, Excel, or image files are allowed.");
        //    }

        //    // Save final official PDF
        //    var officialPath = Path.Combine("wwwroot", "transcripts", pdfFileName);
        //    await System.IO.File.WriteAllBytesAsync(officialPath, officialPdf);

        //    // Watermarked student version
        //    var studentPdf = PdfWatermarkHelper.AddWatermark(officialPdf, "STUDENT COPY");

        //    // Save request update
        //    request.Status = $"Confirmed by Senior Officer, and transcript sent to {request.DestinationEmail}";
        //    request.TranscriptFilePath = officialPath;
        //    await _transcriptRequestService.UpdateTranscriptRequestAsync(request);

        //    // Email content
        //    var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}";
        //    var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
        //    string subject = $"CertiTrack – Request For Transcript – {fullName}";
        //    string body = $@"
        //                    <p>Dear Sir/Madam,</p>
        //                    <p>We are writing to confirm that the academic transcript<br> in respect of {fullName}, including course details, grades, and duration of study,<br> has been verified and forwarded to the institution provided by you.</p>
        //                    <p>Kind regards,<br/>{school.Name}<br/>{DateTime.UtcNow:yyyy-MM-dd}</p>";

        //    try
        //    {
        //        // Send official copy (no watermark)
        //        await _gemailService.SendEmailWithAttachmentAsync(request.DestinationEmail, subject, body, officialPath);

        //        // Send student copy (with watermark)
        //        var studentPath = Path.Combine(tempDir, "Student_" + pdfFileName);
        //        await System.IO.File.WriteAllBytesAsync(studentPath, studentPdf);
        //        await _gemailService.SendEmailWithAttachmentAsync(request.StudentEmail, subject, body, studentPath);

        //        ViewBag.Message = "Transcript emails sent successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        //ModelState.AddModelError(string.Empty, "Failed to send transcript emails. Please try again.");
        //        ViewBag.Message = ex.Message;
        //    }
        //    finally
        //    {
        //        // Cleanup
        //        Directory.Delete(tempDir, true);
        //    }

        //    return Ok(new { success = true });
        //}

        [HttpPost]
        public async Task<IActionResult> ApproveAndSendTranscript(IFormFile transcriptFile, string requestId)
        {
            var request = await _transcriptRequestService.GetTranscriptRequestByIdAsync(long.Parse(requestId));
            if (request == null || request.Status != "Verified")
                return BadRequest("Request not ready or does not exist.");

            if (transcriptFile == null || transcriptFile.Length == 0)
                return BadRequest("Transcript file is required.");

            var ext = Path.GetExtension(transcriptFile.FileName).ToLowerInvariant();
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            var originalFilePath = Path.Combine(tempDir, transcriptFile.FileName);

            using (var stream = new FileStream(originalFilePath, FileMode.Create))
            {
                await transcriptFile.CopyToAsync(stream);
            }

            byte[] originalBytes = await System.IO.File.ReadAllBytesAsync(originalFilePath);
            byte[] officialPdf;
            string pdfFileName = Path.GetFileNameWithoutExtension(transcriptFile.FileName) + ".pdf";

            // Convert to PDF if needed
            if (ext == ".pdf")
            {
                officialPdf = originalBytes;
            }
            else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
            {
                officialPdf = FileToPdfConverter.ConvertImageToPdf(originalBytes);
            }
            else
            {
                return BadRequest("Unsupported file type. Only PDF, Word, Excel, or image files are allowed.");
            }

            // Save official version
            var officialPath = Path.Combine("wwwroot", "transcripts", pdfFileName);
            await System.IO.File.WriteAllBytesAsync(officialPath, officialPdf);

            // Watermarked student version
            var studentPdf = PdfWatermarkHelper.AddWatermark(officialPdf, "STUDENT COPY");
            var studentPath = Path.Combine(tempDir, "Student_" + pdfFileName);
            await System.IO.File.WriteAllBytesAsync(studentPath, studentPdf);

            // Update request
            request.Status = $"Confirmed by Senior Officer, and transcript sent to {request.DestinationEmail}";
            request.TranscriptFilePath = officialPath;
            await _transcriptRequestService.UpdateTranscriptRequestAsync(request);

            var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}";
            var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);

            string subject = $"CertiTrack – Request For Transcript – {fullName}";
            string body = $@"
                            <p>Dear Sir/Madam,</p>
                            <p>We are writing to confirm that the academic transcript<br> in respect of {fullName}, including course details, grades, and duration of study,<br> has been verified and forwarded to the institution provided by you.</p>
                            <p>Kind regards,<br/>{school.Name}<br/>{DateTime.UtcNow:yyyy-MM-dd}</p>";

            try
            {
                // Send official copy (no watermark)
                await _gemailService.SendEmailWithAttachmentAsync(request.DestinationEmail, subject, body, officialPath);

                // Send student copy (with watermark)
                await _gemailService.SendEmailWithAttachmentAsync(request.StudentEmail, subject, body, studentPath);

                ViewBag.Message = "Transcript emails sent successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to send emails: {ex.Message}";
            }
            finally
            {
                // Wait a moment to ensure all file handles are released before deletion
                await Task.Delay(500);

                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Cleanup failed: " + ex.Message);
                    // Optional: log cleanup failure
                }
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

        [HttpGet]
        public async Task<IActionResult> CreateBatchTranscript()
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
            catch (Exception ex)
            {
                return View(ex);
            }
            
        }

            [HttpPost]
        public async Task<IActionResult> CreateBatchTranscript([FromBody] TranscriptRequestBatchVM batchVM)
        {
            var resultMessage = await _transcriptRequestService.BatchInsertAsync(batchVM.TranscriptRequests);

            bool isSuccess = !resultMessage.StartsWith("Error");
            return Json(new { success = isSuccess, message = resultMessage });
        }

        private void AddLookupAndValidation(IXLWorksheet sheet, int column, int rowStart, List<string> values)
        {
            // 1. Insert lookup values under the column (starting from e.g., row 4)
            for (int i = 0; i < values.Count; i++)
            {
                sheet.Cell(rowStart + i, column).Value = values[i];
            }

            // 2. Create named range
            string colLetter = XLHelper.GetColumnLetterFromNumber(column);
            string rangeName = $"Lookup_{colLetter}";
            int rangeStart = rowStart;
            int rangeEnd = rowStart + values.Count - 1;

            var namedRange = sheet.Range($"{colLetter}{rangeStart}:{colLetter}{rangeEnd}");
            sheet.NamedRanges.Add(rangeName, namedRange);

            // 3. Add dropdown validation to the cell (e.g., row 2)
            //var cell = sheet.Cell(2, column);
            //var validation = sheet.Range(cell.Address.ToString()).CreateDataValidation();
            var validationRange = sheet.Range(2, column, 1000, column); // ✅ Correct overload
            var validation = validationRange.CreateDataValidation();

            validation.IgnoreBlanks = true;
            validation.InCellDropdown = true;
            validation.ShowInputMessage = true;
            validation.InputTitle = "Select from list";
            validation.InputMessage = "Choose a valid option from lookup";

            // ✅ Set allowed values using named range
            validation.AllowedValues = XLAllowedValues.List;
            validation.Value = $"={rangeName}";

        }

        private void AddLookupAndValidationWithHiddenSheet(XLWorkbook workbook, IXLWorksheet targetSheet, int targetColumn, string lookupName, List<string> values)
        {
            // 1. Create or get hidden lookup sheet
            var lookupSheetName = "Lookups";
            var lookupSheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == lookupSheetName)
                              ?? workbook.Worksheets.Add(lookupSheetName);
            lookupSheet.Visibility = XLWorksheetVisibility.VeryHidden;

            // 2. Write lookup values to next free column
            int lookupCol = lookupSheet.LastColumnUsed()?.ColumnNumber() + 1 ?? 1;
            int startRow = 1;

            for (int i = 0; i < values.Count; i++)
            {
                lookupSheet.Cell(startRow + i, lookupCol).Value = values[i];
            }

            // 3. Define named range
            string colLetter = XLHelper.GetColumnLetterFromNumber(lookupCol);
            string rangeName = $"Lookup_{lookupName}";
            var range = lookupSheet.Range($"{colLetter}{startRow}:{colLetter}{startRow + values.Count - 1}");

            workbook.NamedRanges.Add(rangeName, range);

            // 4. Apply validation to row 2 to 1000 in the target column
            var validationRange = targetSheet.Range(2, targetColumn, 1000, targetColumn);
            var validation = validationRange.CreateDataValidation();

            validation.IgnoreBlanks = true;
            validation.InCellDropdown = true;
            validation.ShowInputMessage = true;
            validation.InputTitle = "Select from list";
            validation.InputMessage = "Choose from list";
            validation.AllowedValues = XLAllowedValues.List;
            validation.Value = $"={rangeName}";
        }

        [HttpGet("export-transcript-xlsx-template")]
        public async Task<IActionResult> ExportTranscriptXlsxTemplate()
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("CertificateTemplate");

            // Add headers
            sheet.Cell(1, 1).Value = "FirstName";
            sheet.Cell(1, 2).Value = "MiddleName";
            sheet.Cell(1, 3).Value = "LastName";
            sheet.Cell(1, 4).Value = "MatricNumber";
            sheet.Cell(1, 5).Value = "StudentEmail";
            sheet.Cell(1, 6).Value = "DestinationEmail";
            sheet.Cell(1, 7).Value = "Institution";
            sheet.Cell(1, 8).Value = "School";
            sheet.Cell(1, 9).Value = "Faculty";
            sheet.Cell(1, 10).Value = "Department";
            //sheet.Cell(1, 11).Value = "Department";
            //sheet.Cell(1, 12).Value = "School";
            //sheet.Cell(1, 13).Value = "Institution";
            //sheet.Cell(1, 14).Value = "YearOfGraduation";

            // Sample lookup lists — replace with database calls
            //var degrees = new List<string> { "BSc", "MSc", "PhD" };
            //var degreeClasses = new List<string> { "First Class", "Second Class", "Third Class" };
            //var faculties = new List<string> { "Science", "Arts", "Engineering" };
            //var schools = new List<string> { "School A", "School B" };
            //var institutions = new List<string> { "University X", "College Y" };

            var institutions = await _institutionService.GetAllInstitutionsAsync();
            var qualificationTypes = await _qualificationTypeService.GetAllQualificationTypeAsync();
            var qualificationClasses = await _qualificationClassService.GetAllQualificationClassAsync();
            var faculties = await _faculyService.GetAllFacultyAsync();
            var schools = await _schoolService.GetAllSchoolsAsync();
            var departments = await _departmentService.GetAllDepartmentAsync();

            // 3. Fetch lookup values from DB
           // var degreeList = qualificationTypes.Select(d => $"{d.Id} - {d.Name}").ToList();
           // var degreeClassList = qualificationClasses.Select(d => $"{d.Id} - {d.Name}").ToList();
            var facultyList = faculties.Select(f => $"{f.Id} - {f.Name}").ToList();
            var schoolList = schools.Select(s => $"{s.Id} - {s.Name}").ToList();
            var institutionList = institutions.Select(i => $"{i.Id} - {i.Name}").ToList();
            var departmentList = departments.Select(d => $"{d.Name} - {d.Name}").ToList();

            // Add dropdowns using helper method
            //AddLookupAndValidationWithHiddenSheet(workbook, sheet, 8, "Degree", degreeList);
           // AddLookupAndValidationWithHiddenSheet(workbook, sheet, 9, "DegreeClass", degreeClassList);
            
            

            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 7, "Institution", institutionList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 8, "School", schoolList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 9, "Faculty", facultyList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 10, "Department", departmentList);

            // Auto-size columns
            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CertificateUploadTemplate.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> CreateExcelBatchTranscript()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExcelBatchTranscript(IFormFile file)
        {


            if (file == null || file.Length == 0)
                return BadRequest("No file selected");




            var extension = Path.GetExtension(file.FileName).ToLower();

            var transcriptList = new List<TranscriptRequest>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                if (extension == ".csv")
                {
                    using var reader = new StreamReader(stream);
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var values = line.Split(',');

                        if (values.Length >= 5)
                        {
                            transcriptList.Add(new TranscriptRequest
                            {
                                FirstName = values[0],
                                MiddleName = values[1],
                                LastName = values[2],
                                MatricNumber = values[3],
                                StudentEmail = values[4],
                                DestinationEmail = values[5],
                                InstitutionId = long.TryParse(values[6], out long institutionId) ? institutionId : 0,
                                SchoolId = long.TryParse(values[7], out long schoolId) ? schoolId : 0,
                                FacultyId = long.TryParse(values[8], out long facultyId) ? facultyId : 0,
                                Department = values[9],
                                YearOfGraduation = values[10],
                                Status = VerificationStatus.Pending.ToString(),
                                IsPaid = false,
                                TranscriptFilePath = string.Empty,
                                Created = DateTime.Now,
                                Updated = DateTime.Now,
                                CreatedBy = "BulkImport",
                                LastModifiedBy = "BulkImport"
                                // Add other fields as needed
                            });
                        }
                    }
                }
                else if (extension == ".xlsx")
                {
                    using var package = new OfficeOpenXml.ExcelPackage(stream);
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        transcriptList.Add(new TranscriptRequest
                        {
                            FirstName = worksheet.Cells[row, 1].Text,
                            MiddleName = worksheet.Cells[row, 2].Text,
                            LastName = worksheet.Cells[row, 3].Text,
                            MatricNumber = worksheet.Cells[row, 4].Text,
                            StudentEmail = worksheet.Cells[row, 5].Text,
                            DestinationEmail = worksheet.Cells[row, 6].Text,
                            InstitutionId = long.TryParse(worksheet.Cells[row, 7].Text.Split("-")[0], out long institutionId) ? institutionId : 0,
                            SchoolId = long.TryParse(worksheet.Cells[row, 8].Text.Split("-")[0], out long schoolId) ? schoolId : 0,
                            FacultyId = long.TryParse(worksheet.Cells[row, 9].Text.Split("-")[0], out long facultyId) ? facultyId : 0,
                            Department = worksheet.Cells[row, 10].Text.Split("-")[0],
                            YearOfGraduation = worksheet.Cells[row, 11].Text,
                            Status = VerificationStatus.Pending.ToString(),
                            IsPaid = false,
                            TranscriptFilePath = string.Empty,
                            Created = DateTime.Now,
                            Updated = DateTime.Now,
                            CreatedBy = "BulkImport",
                            LastModifiedBy = "BulkImport"
                        });
                    }
                }
                else
                {
                    return BadRequest("Only CSV or Excel (.xlsx) files are supported.");
                }
            }

            // Save to DB
            await _context.TranscriptRequests.AddRangeAsync(transcriptList);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Successfully uploaded {transcriptList.Count} certificate(s).";
            return RedirectToAction("Index");
        }

    }
}
