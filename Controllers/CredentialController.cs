using Certitrack.Data;
using Certitrack.Models;
using Certitrack.Services;
using Certitrack.Services.Certitrack.Services;
using Certitrack.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
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
        private readonly IInstitutionTypeService _institutionTypeService;
        private readonly IDepartmentService _departmentService;
        private readonly ISchoolTypeService _schoolTypeService; 


        public CredentialController(IUserService userService, IRoleService roleService,
            ICertificateDetailService CDService, IIInstitutionService institutionService,
            IQualificationClassService qualificationClassService, IQualificationTypeService qualificationTypeService,
            IFacultyService faculyService, ISchoolService schoolService, GEmailService gemailService, AppDbContext context,
            IInstitutionTypeService institutionTypeService, IDepartmentService departmentService, ISchoolTypeService schoolTypeService  )
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
            _institutionTypeService = institutionTypeService;
            _departmentService = departmentService;
            _schoolTypeService = schoolTypeService;
        }

       


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
                            MatricNo = credential.MatricNo,
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
            var institutionTypes = await _institutionTypeService.GetAllInstitutionTypesAsync(); 
            ViewBag.InstitutionTypes = institutionTypes; 
            var Departments = await _departmentService.GetAllDepartmentAsync();
            ViewBag.Departments = Departments;
            var schoolTypes = await _schoolTypeService.GetAllSchoolTypesAsync();
            ViewBag.SchoolTypes = schoolTypes;  

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
                MatricNo = credentialDetailVM.MatricNo,
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

        [HttpGet]
        public async Task<IActionResult> CreateExcelBatchCredential()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExcelBatchCredential(IFormFile file)
        {
         

            if (file == null || file.Length == 0)
                return BadRequest("No file selected");


           

            var extension = Path.GetExtension(file.FileName).ToLower();

            var certificateList = new List<CertificateDetail>();

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
                            certificateList.Add(new CertificateDetail
                            {
                                HolderFirstName = values[0],
                                HolderMiddleName = values[1],
                                HolderLastName = values[2],
                                CertificateNo = values[3],
                                HolderEmail = values[4],
                                HolderPhoneNumber  = values[5],
                                HolderAddress = values[6],
                                DegreeId = long.TryParse(values[7], out long degreeId) ? degreeId : 0,
                                DegreeClassId = long.TryParse(values[8], out long degreeClassId) ? degreeClassId : 0,
                                FacultyId = long.TryParse(values[9], out long facultyId) ? facultyId : 0,
                                Department = values[10],
                                YearOfGraduation = values[11],
                                SchoolId = long.TryParse(values[12], out long schoolId) ? schoolId : 0,
                                InstitutionId = long.TryParse(values[13], out long institutionId) ? institutionId : 0,
                                MatricNo = values[14],
                                IsVerified = false,
                                Ispaid = false,
                                Status = VerificationStatus.Pending.ToString(),
                                // YearOfGraduation = int.TryParse(values[4], out int year) ? year : 0,
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
                        certificateList.Add(new CertificateDetail
                        {
                            HolderFirstName = worksheet.Cells[row, 1].Text,
                            HolderMiddleName = worksheet.Cells[row, 2].Text,
                            HolderLastName = worksheet.Cells[row, 3].Text,
                            CertificateNo = worksheet.Cells[row, 4].Text,
                            HolderEmail = worksheet.Cells[row, 5].Text,
                            HolderPhoneNumber = worksheet.Cells[row, 6].Text,
                            HolderAddress = worksheet.Cells[row, 7].Text,
                            DegreeId = long.TryParse(worksheet.Cells[row, 8].Text.Split("-")[0], out long degreeId) ? degreeId : 0,
                            DegreeClassId = long.TryParse(worksheet.Cells[row, 9].Text.Split("-")[0], out long degreeClassId) ? degreeClassId : 0,
                            FacultyId = long.TryParse(worksheet.Cells[row, 10].Text.Split("-")[0], out long facultyId) ? facultyId : 0,
                            Department = worksheet.Cells[row, 11].Text.Split("-")[0],
                            
                            SchoolId = long.TryParse(worksheet.Cells[row, 12].Text.Split("-")[0], out long schoolId) ? schoolId : 0,
                            InstitutionId = long.TryParse(worksheet.Cells[row, 13].Text.Split("-")[0], out long institutionId) ? institutionId : 0,
                            YearOfGraduation = worksheet.Cells[row, 14].Text,
                            MatricNo = worksheet.Cells[row, 15].Text,
                            IsVerified = false,
                            Ispaid = false,
                            Status = VerificationStatus.Pending.ToString(),
                            //YearOfGraduation = int.TryParse(worksheet.Cells[row, 5].Text, out int year) ? year : 0,
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
            await _context.CertificateDetails.AddRangeAsync(certificateList);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Successfully uploaded {certificateList.Count} certificate(s).";
            return RedirectToAction("Index");
        }

    //    [HttpGet("export-xlsx-template")]
    //    public async Task<IActionResult> ExportXlsxTemplate()
    //    {
    //        using var workbook = new XLWorkbook();
    //        var sheet = workbook.Worksheets.Add("Template");

    //        // 1. Column headers (row 1)
    //        var headers = new[]
    //        {
    //    "HolderFirstName", "HolderMiddleName", "HolderLastName", "CertificateNo",
    //    "HolderEmail", "HolderPhoneNumber", "HolderAddress",
    //    "Degree", "DegreeClass", "Faculty", "School", "Institution"
    //};

    //        for (int i = 0; i < headers.Length; i++)
    //            sheet.Cell(1, i + 1).Value = headers[i];

    //        // 2. Sample data (row 2)
    //        var sampleData = new[]
    //        {
    //    "John", "M.", "Doe", "CERT123456", "john@example.com", "08012345678", "123 Main Street",
    //    "1", "1", "1", "1", "1"
    //};

    //        for (int i = 0; i < sampleData.Length; i++)
    //            sheet.Cell(2, i + 1).Value = sampleData[i];

    //        var institutions = await _institutionService.GetAllInstitutionsAsync();
    //        var qualificationTypes = await _qualificationTypeService.GetAllQualificationTypeAsync();
    //        var qualificationClasses = await _qualificationClassService.GetAllQualificationClassAsync();
    //        var faculties = await _faculyService.GetAllFacultyAsync();
    //        var schools = await _schoolService.GetAllSchoolsAsync();
        
    //        // 3. Fetch lookup values from DB
    //        var degreeList = qualificationTypes.Select(d => $"{d.Id} - {d.Name}").ToList();
    //        var degreeClassList = qualificationClasses.Select(d => $"{d.Id} - {d.Name}").ToList();
    //        var facultyList = faculties.Select(f => $"{f.Id} - {f.Name}").ToList();
    //        var schoolList = schools.Select(s => $"{s.Id} - {s.Name}").ToList();
    //        var institutionList = institutions.Select(i => $"{i.Id} - {i.Name}").ToList();

    //        // 4. Add lookup values under each column and add dropdown validation (row 2)
    //        AddLookupAndValidation(sheet, column: 8, rowStart: 4, values: degreeList);        // DegreeId (Column H)
    //        AddLookupAndValidation(sheet, column: 9, rowStart: 4, values: degreeClassList);   // DegreeClassId (Column I)
    //        AddLookupAndValidation(sheet, column: 10, rowStart: 4, values: facultyList);      // FacultyId (Column J)
    //        AddLookupAndValidation(sheet, column: 11, rowStart: 4, values: schoolList);       // SchoolId (Column K)
    //        AddLookupAndValidation(sheet, column: 12, rowStart: 4, values: institutionList);  // InstitutionId (Column L)

    //        // Adjust layout
    //        sheet.Columns().AdjustToContents();

    //        // Stream and return
    //        using var stream = new MemoryStream();
    //        workbook.SaveAs(stream);
    //        stream.Seek(0, SeekOrigin.Begin);
    //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadTemplate.xlsx");
    //    }

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

        [HttpGet("export-xlsx-template")]
        public async Task<IActionResult> ExportXlsxTemplate()
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("CertificateTemplate");

            // Add headers
            sheet.Cell(1, 1).Value = "HolderFirstName";
            sheet.Cell(1, 2).Value = "HolderMiddleName";
            sheet.Cell(1, 3).Value = "HolderLastName";
            sheet.Cell(1, 4).Value = "CertificateNo";
            sheet.Cell(1, 5).Value = "HolderEmail";
            sheet.Cell(1, 6).Value = "HolderPhoneNumber";
            sheet.Cell(1, 7).Value = "HolderAddress";
            sheet.Cell(1, 8).Value = "Degree";
            sheet.Cell(1, 9).Value = "DegreeClass";
            sheet.Cell(1, 10).Value = "Faculty";
            sheet.Cell(1, 11).Value = "Department";
            sheet.Cell(1, 12).Value = "School";
            sheet.Cell(1, 13).Value = "Institution";
            sheet.Cell(1, 14).Value = "YearOfGraduation";
            sheet.Cell(1, 15).Value = "MatricNo";

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
            var degreeList = qualificationTypes.Select(d => $"{d.Id} - {d.Name}").ToList();
            var degreeClassList = qualificationClasses.Select(d => $"{d.Id} - {d.Name}").ToList();
            var facultyList = faculties.Select(f => $"{f.Id} - {f.Name}").ToList();
            var schoolList = schools.Select(s => $"{s.Id} - {s.Name}").ToList();
            var institutionList = institutions.Select(i => $"{i.Id} - {i.Name}").ToList();
            var departmentList = departments.Select(d => $"{d.Name} - {d.Name}").ToList();

            // Add dropdowns using helper method
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 8, "Degree", degreeList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 9, "DegreeClass", degreeClassList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 10, "Faculty", facultyList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 11, "Department", departmentList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 12, "School", schoolList);
            AddLookupAndValidationWithHiddenSheet(workbook, sheet, 13, "Institution", institutionList);

            // Auto-size columns
            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CertificateUploadTemplate.xlsx");
        }


    }
}
