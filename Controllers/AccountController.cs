using Certitrack.Models;
using Certitrack.Services;
using Certitrack.Services.Certitrack.Services;
using Certitrack.Utility;
using Certitrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Certitrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly GEmailService _gemailService;
        private readonly IUserRegistrationInviteService _userRegistrationInviteService;
        private readonly ITitleService _titleService;
        private readonly IInstitutionTypeService _institutionTypeService;
        private readonly IIInstitutionService _institutionService;
        private readonly ISchoolService _schoolService;
        private readonly ICertificateDetailService _certificateDetailService;
        private readonly ITranscriptRequestService _transcriptRequestService;
        private readonly IFacultyService _faculyService;
        private readonly IPaymentService _paymentService;

        public AccountController(IUserService userService, IRoleService roleService,
            GEmailService gemailService, IUserRegistrationInviteService userRegistrationInviteService,
            ITitleService titleService, IInstitutionTypeService institutionTypeService, IIInstitutionService institutionService,
            ISchoolService schoolService, ICertificateDetailService certificateDetailService, ITranscriptRequestService transcriptRequestService,
            IFacultyService faculyService, IPaymentService paymentService)
        {
            _userService = userService;
            _roleService = roleService;
            _gemailService = gemailService;
            _userRegistrationInviteService = userRegistrationInviteService;
            _titleService = titleService;
            _institutionTypeService = institutionTypeService;
            _institutionService = institutionService;
            _schoolService = schoolService;
            _certificateDetailService = certificateDetailService;
            _transcriptRequestService = transcriptRequestService;
            _faculyService = faculyService;
            _paymentService = paymentService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            var userInvite = await _userRegistrationInviteService.GetAlllUserRegistrationInvitesAsync();
            // Assuming you want to check for active accounts

            var InviteUserList = new List<InviteUserIdexVM>();

            var roleCache = new Dictionary<long, string>();

            if (userInvite == null)
            {
                TempData["Message"] = "No user invites found.";
                return View();
            }
            else
            {
                foreach (var invite in userInvite)
                {
                    if (!roleCache.TryGetValue(invite.RoleId, out var roleName))
                    {
                        var role = await _roleService.GetRoleByIdAsync(invite.RoleId);
                        roleName = role?.Name ?? "N/A";
                        roleCache[invite.RoleId] = roleName;
                    }

                    var inviteExists = (await _userService.GetAllUsersAsync()).Any(u => u.InviteId == invite.Id);

                    if (inviteExists)
                    {
                        var accountStatus = (await _userService.GetAllUsersAsync()).FirstOrDefault(u => u.InviteId == invite.Id).IsActive;
                        InviteUserList.Add(new InviteUserIdexVM
                        {
                            Email = invite.Email,
                            RoleName = roleName,
                            Created = invite.Created,
                            ExpiresAt = invite.ExpiresAt,
                            Token = invite.Token,
                            OrganizationName = invite.OrganizationName,
                            OrganizationAddress = invite.OrganizationAddress,
                            OrganizationPhone = invite.OrganizationPhone,
                            OrganizationEmail = invite.OrganizationEmail,
                            IsActive = accountStatus,
                            activationlink = invite.activationlink
                        });
                    }
                    else
                    {
                        InviteUserList.Add(new InviteUserIdexVM
                        {
                            Email = invite.Email,
                            RoleName = roleName,
                            Created = invite.Created,
                            ExpiresAt = invite.ExpiresAt,
                            Token = invite.Token,
                            OrganizationName = invite.OrganizationName == null ? "" : invite.OrganizationName,
                            OrganizationAddress = invite.OrganizationAddress == null ? "" : invite.OrganizationAddress,
                            OrganizationPhone = invite.OrganizationPhone == null ? "" : invite.OrganizationPhone,
                            OrganizationEmail = invite.OrganizationEmail == null ? "" : invite.OrganizationEmail,
                            IsActive = false,
                            activationlink = invite.activationlink
                        });
                    }



                }
            }


            var pagedList = InviteUserList.OrderBy(u => u.Email).ToList().ToPagedList(pageNumber, pageSize);

            // Make it IEnumerable



            return View(pagedList);
        }
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        //{
        //    var FreeEmailRegex = new Regex(@"@(gmail|yahoo|outlook|hotmail|live|aol|icloud|mail|protonmail|zoho)\.(com|net|org|co\.uk|ca|in)$", RegexOptions.IgnoreCase);
        //    var ValidEmailFormatRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");
        //    var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>])[A-Za-z\d!@#$%^&*(),.?""{}|<>]{8,}$", RegexOptions.IgnoreCase); // Example regex for password validation
        //    //var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?\":{ }|<>])[A-Za - z\d!@#$%^&*(),.?\":{}|<>]{8,}$");


        //    if (!ModelState.IsValid)
        //        return BadRequest(new { success = false, message = "Invalid login data" });



        //    if (!ValidEmailFormatRegex.IsMatch(model.Email))
        //    {
        //        return BadRequest(new { success = false, message = "Invalid email format." });
        //    }

        //    bool isValidPassword = passwordRegex.IsMatch(model.Password);
        //    if (!isValidPassword)
        //    {
        //        return BadRequest(new { success = false, message = "Password must be at least 8 characters long, contain at least one uppercase letter, one digit, and one special character." });
        //    }




        //    //Password@123
        //    var user = await _userService.AuthenticateAsync(model.Email, HashPassword(model.Password));

        //        {

        //            bool isFreeEmail = FreeEmailRegex.IsMatch(model.Email);
        //            if (isFreeEmail)
        //            {
        //                var domain = model.Email.Split('@').Last().ToLower();
        //                return BadRequest(new { success = false, message = $"Emails from '{domain}' are not allowed. Please use your organization's email address." });
        //            }
        //        }


        //    if (user != null && user.IsActive)
        //    {
        //        var invite = (await _userRegistrationInviteService.GetAlllUserRegistrationInvitesAsync())
        //     .FirstOrDefault(ui => ui.Id == user.InviteId);


        //        HttpContext.Session.SetString("UserEmail", model.Email);
        //        // Combine Firstname, Middlename, and Lastname into a full name (handle possible nulls)
        //        var fullName = $"{user.FirstName} {user.MiddleName} {user.LastName}".Trim();
        //        var role = await _roleService.GetRoleByIdAsync(user.RoleId);
        //        HttpContext.Session.SetString("UserFullName", fullName);
        //        HttpContext.Session.SetString("UserRole", user.RoleId.ToString());
        //        HttpContext.Session.SetString("UserRoleName", role.Name.ToString());
        //        HttpContext.Session.SetString("UserId", user.Id.ToString());
        //        HttpContext.Session.SetString("InstitutionType", invite?.TypeId?.ToString() ?? string.Empty);
        //        HttpContext.Session.SetString("SchoolId", invite?.SchoolId?.ToString() ?? string.Empty);
        //        HttpContext.Session.SetString("InstitutionId", invite?.OrganizationName ?? string.Empty);
        //        return Ok(new { success = true, message = "Login successful" });
        //    }

        //    return Unauthorized(new { success = false, message = "Invalid email or password or User Account Disabled" });
        //}

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var FreeEmailRegex = new Regex(@"@(gmail|yahoo|outlook|hotmail|live|aol|icloud|mail|protonmail|zoho)\.(com|net|org|co\.uk|ca|in)$", RegexOptions.IgnoreCase);
            var ValidEmailFormatRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>])[A-Za-z\d!@#$%^&*(),.?""{}|<>]{8,}$");

            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid login data" });

            if (!ValidEmailFormatRegex.IsMatch(model.Email))
                return BadRequest(new { success = false, message = "Invalid email format." });

            if (!passwordRegex.IsMatch(model.Password))
                return BadRequest(new { success = false, message = "Password must be at least 8 characters long, contain at least one uppercase letter, one digit, and one special character." });

            // Get user by email first
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { success = false, message = "Invalid email or password" });

            if (user.IsLocked)
            {
                var minutesLeft = (user.LockoutEnd.Value - DateTime.UtcNow).Minutes;
                return Unauthorized(new { success = false, message = $"Account is locked. Try again in {minutesLeft} minute(s)." });
            }

            // Verify password manually (not using AuthenticateAsync anymore)
            // var isPasswordValid = user.PasswordHash == HashPassword(model.Password);
            var isPasswordValid = VerifyPassword(model.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 3)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15); // lock for 15 minutes
                    await _userService.UpdateUserAsync(user);

                    return Unauthorized(new
                    {
                        success = false,
                        message = "Account locked after 3 failed attempts. Try again in 15 minutes."
                    });
                }
                else
                {
                    await _userService.UpdateUserAsync(user);
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Invalid email or password"
                    });
                }
            }

            if (!user.IsActive)
                return Unauthorized(new { success = false, message = "User account is disabled" });

            if (user.RoleId != 3 && user.RoleId != 7 && FreeEmailRegex.IsMatch(model.Email))
            {
                var domain = model.Email.Split('@').Last().ToLower();
                return BadRequest(new { success = false, message = $"Emails from '{domain}' are not allowed. Please use your organization's email address." });
            }

            // Login successful: reset failed attempts
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _userService.UpdateUserAsync(user);

            var invite = (await _userRegistrationInviteService.GetAlllUserRegistrationInvitesAsync())
                .FirstOrDefault(ui => ui.Id == user.InviteId);

            var fullName = $"{user.FirstName} {user.MiddleName} {user.LastName}".Trim();
            var role = await _roleService.GetRoleByIdAsync(user.RoleId);

            HttpContext.Session.SetString("UserEmail", model.Email);
            HttpContext.Session.SetString("UserFullName", fullName);
            HttpContext.Session.SetString("UserRole", user.RoleId.ToString());
            HttpContext.Session.SetString("UserRoleName", role.Name.ToString());
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("InstitutionType", invite?.TypeId?.ToString() ?? string.Empty);
            HttpContext.Session.SetString("SchoolId", invite?.SchoolId?.ToString() ?? string.Empty);
            HttpContext.Session.SetString("InstitutionId", invite?.OrganizationName ?? string.Empty);

            return Ok(new { success = true, message = "Login successful" });
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { success = true, message = "Logged out" });
        }

        public async Task<IActionResult> Dashboard()
        {
            string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
            string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
            string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
            string roleName = HttpContext?.Session?.GetString("UserRoleName");
            var UserEmail = HttpContext?.Session?.GetString("UserEmail");

            if (roleName == "SuperAdmin" || roleName == "AdminUser")
            {
                var schools = await _schoolService.GetAllSchoolsAsync();
                var institutions = await _institutionService.GetAllInstitutionsAsync();
                var faculties = await _faculyService.GetAllFacultyAsync();

                ViewBag.Schools = schools;
                ViewBag.Institutions = institutions;
                ViewBag.Faculties = faculties;
            }
            else if (roleName == "Agent" || roleName == "Student")
            {
                ViewBag.Schools = Enumerable.Empty<School>();
                ViewBag.Institutions = Enumerable.Empty<Institution>();
                ViewBag.Faculties = Enumerable.Empty<Faculty>();
            }
            else
            {
                if (institutionType == "3")
                {
                    var school = await _schoolService.GetSchoolByIdAsync(long.Parse(schoolId));
                    var schools = school != null ? new List<School> { school } : Enumerable.Empty<School>();

                    var faculties = await _faculyService.GetAllFacultyAsync();

                    ViewBag.Schools = schools;
                    ViewBag.Institutions = Enumerable.Empty<Institution>();
                    ViewBag.Faculties = faculties;
                }
                else
                {
                    var institution = await _institutionService.GetInstitutionByIdAsync(long.Parse(institutionId));
                    var institutions = institution != null ? new List<Institution> { institution } : Enumerable.Empty<Institution>();

                    ViewBag.Schools = Enumerable.Empty<School>();
                    ViewBag.Institutions = institutions;
                    ViewBag.Faculties = Enumerable.Empty<Faculty>();
                }
            }

            return View(); // Dashboard page
        }

        //public async Task<IActionResult> Dashboard()
        //{
        //    string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;

        //    string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
        //    string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
        //    string roleName = HttpContext?.Session?.GetString("UserRoleName");
        //    var UserEmail = HttpContext?.Session?.GetString("UserEmail");



        //    if (roleName == "SuperAdmin" || roleName == "AdminUser")
        //    {
        //        var schools = await _schoolService.GetAllSchoolsAsync();
        //        var institutions = await _institutionService.GetAllInstitutionsAsync();
        //        var faculties = await _faculyService.GetAllFacultyAsync();
        //        ViewBag.Schools = schools;
        //        ViewBag.Institutions = institutions;
        //        ViewBag.Faculties = faculties;
        //    }
        //    else if(roleName == "Agent")
        //    {
        //        var schools = Enumerable.Empty<School>();
        //        var institutions = Enumerable.Empty<Institution>();
        //        var faculties = Enumerable.Empty<Faculty>();
        //        ViewBag.Schools = schools;
        //        ViewBag.Institutions = institutions;
        //        ViewBag.Faculties = faculties;
        //    }
        //    else if (roleName == "Student")
        //    {
        //        var schools = Enumerable.Empty<School>();
        //        var institutions = Enumerable.Empty<Institution>();
        //        var faculties = Enumerable.Empty<Faculty>();
        //        ViewBag.Schools = schools;
        //        ViewBag.Institutions = institutions;
        //        ViewBag.Faculties = faculties;
        //    }
        //    else
        //    {
        //        if (institutionType == "3")
        //        {
        //            var schools = await _schoolService.GetSchoolByIdAsync(long.Parse(schoolId));
        //            var institutions = Enumerable.Empty<Institution>();
        //            var faculties = await _faculyService.GetAllFacultyAsync();
        //            ViewBag.Schools = schools;
        //            ViewBag.Institutions = institutions;
        //            ViewBag.Faculties = faculties;
        //        }
        //        else
        //        {
        //            var schools = Enumerable.Empty<School>();
        //            var institutions = await _institutionService.GetInstitutionByIdAsync(long.Parse(institutionId));
        //            var faculties = Enumerable.Empty<Faculty>();
        //            ViewBag.Schools = schools;
        //            ViewBag.Institutions = institutions;
        //            ViewBag.Faculties = faculties;
        //        }


        //    }




        //        return View(); // This is your dashboard page
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetDashboardData()
        //{
        //    var totalCredentials = await _certificateDetailService.GetAllCertificateDetailAsync();
        //    var totalTranscriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
        //    var payments = await _paymentService.GetAllPaymentAsync();

        //    var dashboardData = new DashboardViewModel
        //    {
        //        TotalCredentials = totalCredentials.Count(),
        //        TotalTranscriptRequests = totalTranscriptRequests.Count(),
        //        TotalProcessedRequests = 0
        //    };

        //    return Json(dashboardData);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetDashboardData(DateTime? startDate, DateTime? endDate, int? institutionId, int? schoolId)
        //{
        //    var totalCredentials = await _certificateDetailService.GetAllCertificateDetailAsync();
        //    var totalTranscriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
        //    var payments = await _paymentService.GetAllPaymentAsync();

        //    if (startDate.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.Created >= startDate.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.Created >= startDate.Value).ToList();
        //        payments = payments.Where(p => p.TransactionDate >= startDate.Value).ToList();
        //    }

        //    if (endDate.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.Created >= endDate.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.Created >= endDate.Value).ToList();
        //        payments = payments.Where(p => p.TransactionDate >= endDate.Value).ToList();
        //    }

        //    if (institutionId.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.InstitutionId >= institutionId.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.InstitutionId >= institutionId.Value).ToList();
        //       // payments = payments.Where(p => p.InstitutionId == institutionId).ToList();
        //    }

        //    if (schoolId.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.Created >= startDate.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.Created >= startDate.Value).ToList();
        //        // payments = payments.Where(p => p.SchoolId == schoolId).ToList();
        //    }

        //    var processedRequests = totalCredentials.Count(c => c.Status != "Pending") + totalTranscriptRequests.Count(t => t.Status != "Pending");

        //    // Trend Data
        //    var trendDates = totalCredentials
        //        .Select(c => c.Created.Date)
        //        .Union(totalTranscriptRequests.Select(t => t.Created.Date))
        //        .Distinct()
        //        .OrderBy(d => d)
        //        .ToList();

        //    var trendCredentials = trendDates.Select(d => totalCredentials.Count(c => c.Created.Date == d)).ToList();
        //    var trendTranscripts = trendDates.Select(d => totalTranscriptRequests.Count(t => t.Created.Date == d)).ToList();

        //    // Payment Trend Data
        //    var paymentTrendData = payments
        //        .GroupBy(p => p.TransactionDate.Date)
        //        .Select(g => new { Date = g.Key, TotalAmount = g.Sum(p => p.Amount) })
        //        .OrderBy(p => p.Date)
        //        .ToList();

        //    var paymentTrendDates = paymentTrendData.Select(p => p.Date.ToString("yyyy-MM-dd")).ToList();
        //    var paymentTrendAmounts = paymentTrendData.Select(p => p.TotalAmount).ToList();
        //   // var credentialPayments = paymentTrendData.Select(p => p.), // payments with PaymentType = 'C'
        //   // var transcriptPayments = transcriptPaymentAmounts  // payments with PaymentType = 'T'

        //    return Json(new
        //    {
        //        totalCredentials = trendCredentials.Count,
        //        totalTranscriptRequests = trendTranscripts.Count,
        //        totalProcessedRequests = processedRequests,
        //        trendDates = trendDates.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
        //        trendCredentials,
        //        trendTranscripts,
        //        paymentTrendDates,
        //        paymentTrendAmounts

        //    });
        //}

        [HttpGet]
        public async Task<IActionResult> GetDashboardData(DateTime? startDate, DateTime? endDate, int? institutionId, int? schoolId)
        {
            string roleName = HttpContext?.Session?.GetString("UserRoleName");
            string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
            int schoolIdFromSession = int.Parse(HttpContext?.Session?.GetString("SchoolId") ?? "0");
            int institutionIdFromSession = int.Parse(HttpContext?.Session?.GetString("InstitutionId") ?? "0");

            var totalCredentials = await _certificateDetailService.GetAllCertificateDetailAsync();
            var totalTranscriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
            var payments = await _paymentService.GetAllPaymentAsync();
            var transcriptPayments = payments.Where(p => p.PaymentType == "T").ToList();
            var credentialPayments = payments.Where(p => p.PaymentType == "C").ToList();
            var totalCredentialPayments = credentialPayments.Sum(p => p.Amount);
            var totalTranscriptPayments = transcriptPayments.Sum(p => p.Amount);
            var totalPayments = payments.Sum(p => p.Amount); // Total payments across all types  

            // 🔑 Restrict data by role (before any filters)
            if (!(roleName == "SuperAdmin" || roleName == "AdminUser"))
            {
                if (institutionType == "3") // School-based role
                {
                    totalCredentials = totalCredentials.Where(c => c.SchoolId == schoolIdFromSession).ToList();
                    totalTranscriptRequests = totalTranscriptRequests.Where(t => t.SchoolId == schoolIdFromSession).ToList();

                    totalCredentialPayments = totalCredentials.ToList().Count() * 200000;
                    totalTranscriptPayments = totalTranscriptRequests.ToList().Count() * 300000;
                    totalPayments = totalCredentialPayments + totalTranscriptPayments; // Assuming PaymentAmount is a property in CertificateDetail
                    // Assuming PaymentAmount is a property in CertificateDetail
                }
                else // Institution-based role
                {
                    totalCredentials = totalCredentials.Where(c => c.InstitutionId == institutionIdFromSession).ToList();
                    totalTranscriptRequests = totalTranscriptRequests.Where(t => t.InstitutionId == institutionIdFromSession).ToList();
                    // payments = payments.Where(p => p.InstitutionId == institutionIdFromSession).ToList();
                    totalCredentialPayments = totalCredentials.ToList().Count() * 200000;
                    totalTranscriptPayments = totalTranscriptRequests.ToList().Count() * 300000;
                    totalPayments = totalCredentialPayments + totalTranscriptPayments; // Assuming PaymentAmount is a property in CertificateDetail
                }
            }

            // Strip time from incoming dates
            startDate = startDate?.Date;
            endDate = endDate?.Date;

            // Date filtering
            if (startDate.HasValue && endDate.HasValue)
            {
                totalCredentials = totalCredentials
                    .Where(c => c.Created.Date >= startDate.Value && c.Created.Date <= endDate.Value)
                    .ToList();

                totalTranscriptRequests = totalTranscriptRequests
                    .Where(t => t.Created.Date >= startDate.Value && t.Created.Date <= endDate.Value)
                    .ToList();

                payments = payments
                    .Where(p => p.TransactionDate.Date >= startDate.Value && p.TransactionDate.Date <= endDate.Value)
                    .ToList();
            }
            else if (startDate.HasValue)
            {
                totalCredentials = totalCredentials.Where(c => c.Created.Date == startDate.Value).ToList();
                totalTranscriptRequests = totalTranscriptRequests.Where(t => t.Created.Date == startDate.Value).ToList();
                payments = payments.Where(p => p.TransactionDate.Date == startDate.Value).ToList();
            }
            else if (endDate.HasValue)
            {
                totalCredentials = totalCredentials.Where(c => c.Created.Date == endDate.Value).ToList();
                totalTranscriptRequests = totalTranscriptRequests.Where(t => t.Created.Date == endDate.Value).ToList();
                payments = payments.Where(p => p.TransactionDate.Date == endDate.Value).ToList();
            }

            // Institution filter (manual override only applies to Admin roles)
            if ((roleName == "SuperAdmin" || roleName == "AdminUser") && institutionId.HasValue)
            {
                totalCredentials = totalCredentials.Where(c => c.InstitutionId == institutionId.Value).ToList();
                totalTranscriptRequests = totalTranscriptRequests.Where(t => t.InstitutionId == institutionId.Value).ToList();
            }

            // School filter (manual override only applies to Admin roles)
            if ((roleName == "SuperAdmin" || roleName == "AdminUser") && schoolId.HasValue)
            {
                totalCredentials = totalCredentials.Where(c => c.SchoolId == schoolId.Value).ToList();
                totalTranscriptRequests = totalTranscriptRequests.Where(t => t.SchoolId == schoolId.Value).ToList();
            }

            var processedRequests = totalCredentials.Count(c => c.Status != "Pending") +
                                    totalTranscriptRequests.Count(t => t.Status != "Pending");

            // Trend Data
            var trendDates = totalCredentials.Select(c => c.Created.Date)
                .Union(totalTranscriptRequests.Select(t => t.Created.Date))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var trendCredentials = trendDates.Select(d => totalCredentials.Count(c => c.Created.Date == d)).ToList();
            var trendTranscripts = trendDates.Select(d => totalTranscriptRequests.Count(t => t.Created.Date == d)).ToList();

            // Payment Trend Data
            var paymentTrendData = payments
                .GroupBy(p => p.TransactionDate.Date)
                .Select(g => new { Date = g.Key, TotalAmount = g.Sum(p => p.Amount) })
                .OrderBy(p => p.Date)
                .ToList();

            var paymentTrendDates = paymentTrendData.Select(p => p.Date.ToString("yyyy-MM-dd")).ToList();
            var paymentTrendAmounts = paymentTrendData.Select(p => p.TotalAmount).ToList();

            return Json(new
            {
                totalCredentials = totalCredentials.Count(),
                totalTranscriptRequests = totalTranscriptRequests.Count(),
                totalProcessedRequests = processedRequests,
                trendDates = trendDates.Select(d => d.ToString("yyyy-MM-dd")).ToList(),

                totalCredentialPayments,
                totalTranscriptPayments,
                totalPayments, // Total payments across all types  


                trendCredentials,
                trendTranscripts,
                paymentTrendDates,
                paymentTrendAmounts
            });
        }


        //[HttpGet]
        //public async Task<IActionResult> GetDashboardData(DateTime? startDate, DateTime? endDate, int? institutionId, int? schoolId)
        //{
        //    var totalCredentials = await _certificateDetailService.GetAllCertificateDetailAsync();
        //    var totalTranscriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
        //    var payments = await _paymentService.GetAllPaymentAsync();

        //    // Strip time from incoming dates
        //    startDate = startDate?.Date;
        //    endDate = endDate?.Date;

        //    // Date filtering
        //    if (startDate.HasValue && endDate.HasValue)
        //    {
        //        totalCredentials = totalCredentials
        //            .Where(c => c.Created.Date >= startDate.Value && c.Created.Date <= endDate.Value)
        //            .ToList();

        //        totalTranscriptRequests = totalTranscriptRequests
        //            .Where(t => t.Created.Date >= startDate.Value && t.Created.Date <= endDate.Value)
        //            .ToList();

        //        payments = payments
        //            .Where(p => p.TransactionDate.Date >= startDate.Value && p.TransactionDate.Date <= endDate.Value)
        //            .ToList();
        //    }
        //    else if (startDate.HasValue)
        //    {
        //        totalCredentials = totalCredentials
        //            .Where(c => c.Created.Date == startDate.Value)
        //            .ToList();

        //        totalTranscriptRequests = totalTranscriptRequests
        //            .Where(t => t.Created.Date == startDate.Value)
        //            .ToList();

        //        payments = payments
        //            .Where(p => p.TransactionDate.Date == startDate.Value)
        //            .ToList();
        //    }
        //    else if (endDate.HasValue)
        //    {
        //        totalCredentials = totalCredentials
        //            .Where(c => c.Created.Date == endDate.Value)
        //            .ToList();

        //        totalTranscriptRequests = totalTranscriptRequests
        //            .Where(t => t.Created.Date == endDate.Value)
        //            .ToList();

        //        payments = payments
        //            .Where(p => p.TransactionDate.Date == endDate.Value)
        //            .ToList();
        //    }

        //    // Institution filter
        //    if (institutionId.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.InstitutionId == institutionId.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.InstitutionId == institutionId.Value).ToList();
        //    }

        //    // School filter
        //    if (schoolId.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.SchoolId == schoolId.Value).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.SchoolId == schoolId.Value).ToList();
        //    }

        //    var processedRequests = totalCredentials.Count(c => c.Status != "Pending") +
        //                            totalTranscriptRequests.Count(t => t.Status != "Pending");

        //    // Trend Data
        //    var trendDates = totalCredentials.Select(c => c.Created.Date)
        //        .Union(totalTranscriptRequests.Select(t => t.Created.Date))
        //        .Distinct()
        //        .OrderBy(d => d)
        //        .ToList();

        //    var trendCredentials = trendDates.Select(d => totalCredentials.Count(c => c.Created.Date == d)).ToList();
        //    var trendTranscripts = trendDates.Select(d => totalTranscriptRequests.Count(t => t.Created.Date == d)).ToList();

        //    // Payment Trend Data
        //    var paymentTrendData = payments
        //        .GroupBy(p => p.TransactionDate.Date)
        //        .Select(g => new { Date = g.Key, TotalAmount = g.Sum(p => p.Amount) })
        //        .OrderBy(p => p.Date)
        //        .ToList();

        //    var paymentTrendDates = paymentTrendData.Select(p => p.Date.ToString("yyyy-MM-dd")).ToList();
        //    var paymentTrendAmounts = paymentTrendData.Select(p => p.TotalAmount).ToList();

        //    return Json(new
        //    {
        //        totalCredentials = totalCredentials.Count(),
        //        totalTranscriptRequests = totalTranscriptRequests.Count(),
        //        totalProcessedRequests = processedRequests,
        //        trendDates = trendDates.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
        //        trendCredentials,
        //        trendTranscripts,
        //        paymentTrendDates,
        //        paymentTrendAmounts
        //    });
        //}

        //public async Task<IActionResult> GetDashboardData(DateTime? startDate, DateTime? endDate, int? institutionId, int? schoolId)
        //{
        //    var totalCredentials = await _certificateDetailService.GetAllCertificateDetailAsync();
        //    var totalTranscriptRequests = await _transcriptRequestService.GetAllTranscriptRequestAsync();
        //    var payments = await _paymentService.GetAllPaymentAsync();

        //    Normalize dates(strip time)
        //    startDate = startDate?.Date;
        //    endDate = endDate?.Date;

        //    Helper for filtering by date

        //   Func<DateTime, bool> dateFilter = date =>
        //   {
        //       var d = date.Date;
        //       if (startDate.HasValue && endDate.HasValue)
        //           return d >= startDate.Value && d <= endDate.Value;
        //       if (startDate.HasValue)
        //           return d >= startDate.Value;
        //       if (endDate.HasValue)
        //           return d <= endDate.Value;
        //       return true; // no filter
        //   };

        //    Apply date filter

        //   totalCredentials = totalCredentials.Where(c => dateFilter(c.Created)).ToList();
        //    totalTranscriptRequests = totalTranscriptRequests.Where(t => dateFilter(t.Created)).ToList();
        //    payments = payments.Where(p => dateFilter(p.TransactionDate)).ToList();

        //    Apply Institution filter
        //    if (institutionId.HasValue)
        //        {
        //            totalCredentials = totalCredentials.Where(c => c.InstitutionId == institutionId).ToList();
        //            totalTranscriptRequests = totalTranscriptRequests.Where(t => t.InstitutionId == institutionId).ToList();
        //        }

        //    Apply School filter
        //    if (schoolId.HasValue)
        //    {
        //        totalCredentials = totalCredentials.Where(c => c.SchoolId == schoolId).ToList();
        //        totalTranscriptRequests = totalTranscriptRequests.Where(t => t.SchoolId == schoolId).ToList();
        //    }

        //    var processedRequests = totalCredentials.Count(c => c.Status != "Pending") +
        //                            totalTranscriptRequests.Count(t => t.Status != "Pending");

        //    Trend Data
        //    var trendDates = totalCredentials.Select(c => c.Created.Date)
        //        .Union(totalTranscriptRequests.Select(t => t.Created.Date))
        //        .Distinct()
        //        .OrderBy(d => d)
        //        .ToList();

        //    var trendCredentials = trendDates.Select(d => totalCredentials.Count(c => c.Created.Date == d)).ToList();
        //    var trendTranscripts = trendDates.Select(d => totalTranscriptRequests.Count(t => t.Created.Date == d)).ToList();

        //    Payment Trend Data
        //   var paymentTrendData = payments
        //       .GroupBy(p => p.TransactionDate.Date)
        //       .Select(g => new { Date = g.Key, TotalAmount = g.Sum(p => p.Amount) })
        //       .OrderBy(p => p.Date)
        //       .ToList();

        //    var paymentTrendDates = paymentTrendData.Select(p => p.Date.ToString("yyyy-MM-dd")).ToList();
        //    var paymentTrendAmounts = paymentTrendData.Select(p => p.TotalAmount).ToList();

        //    return Json(new
        //    {
        //        totalCredentials = totalCredentials.Count(),
        //        totalTranscriptRequests = totalTranscriptRequests.Count(),
        //        totalProcessedRequests = processedRequests,
        //        trendDates = trendDates.Select(d => d.ToString("yyyy-MM-dd")).ToList(),
        //        trendCredentials,
        //        trendTranscripts,
        //        paymentTrendDates,
        //        paymentTrendAmounts
        //    });
        //}




        public string HashPassword(string password)
        {
            try
            {
                // Generate salt and hash password
                string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor: 12); // Example: work factor of 12
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
                Console.WriteLine($"Generated Hash: {hashedPassword}"); // Debugging line
                return hashedPassword;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hashing password: {ex.Message}");
                throw;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Verify password
                bool isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                Console.WriteLine($"Password verification result: {isValid}"); // Debugging line
                return isValid;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                Console.WriteLine($"Invalid salt version: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying password: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> InviteUser()
        {
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            ViewBag.Types = await _institutionTypeService.GetAllInstitutionTypesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(InviteUserVM model)
        {
            var FreeEmailRegex = new Regex(@"@(gmail|yahoo|outlook|hotmail|live|aol|icloud|mail|protonmail|zoho)\.(com|net|org|co\.uk|ca|in)$", RegexOptions.IgnoreCase);
            var ValidEmailFormatRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleService.GetAllRolesAsync();
                ViewBag.Types = await _institutionTypeService.GetAllInstitutionTypesAsync();
                return View(model);
            }


            // var emailDomain = model.Email.Split('@').Last().ToLower();

            if (model.RoleId != 7)
            {
                bool isFreeEmail = FreeEmailRegex.IsMatch(model.Email);
                if (isFreeEmail)
                {
                    var domain = model.Email.Split('@').Last().ToLower();
                    ViewBag.Message = $"Emails from '{domain}' are not allowed. Please use your organization's email address.";
                    ViewBag.Roles = await _roleService.GetAllRolesAsync();
                    ViewBag.Types = await _institutionTypeService.GetAllInstitutionTypesAsync();
                    return View(model);
                    //return BadRequest(new { success = false, message = $"Emails from '{domain}' are not allowed. Please use your organization's email address." });
                }
            }



            if (!ValidEmailFormatRegex.IsMatch(model.Email))
            {
                //return BadRequest(new { success = false, message = "Invalid email format." });

                ViewBag.Message = $"Invalid email format.";
                ViewBag.Roles = await _roleService.GetAllRolesAsync();
                ViewBag.Types = await _institutionTypeService.GetAllInstitutionTypesAsync();
                return View(model);
            }

            // Save user invite logic here (if applicable)
            // e.g., create a user with a generated token, role, etc.

            // Example token generation
            string token = Guid.NewGuid().ToString(); // store this with the user/invite for later verification
            string activationLink = Url.Action("ActivateAccount", "Account", new { token = token }, Request.Scheme);

            var userRegistrationInvite = new UserRegistrationInvite
            {

                RoleId = model.RoleId,
                Email = model.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24), // Set expiration time
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                CreatedBy = HttpContext.Session.GetString("UserEmail"), // Assuming you have a way to get the current user's email
                LastModifiedBy = HttpContext.Session.GetString("UserEmail"),
                TypeId = model.TypeId,
                activationlink = activationLink


            };

            await _userRegistrationInviteService.CreateInviteAsync(userRegistrationInvite);




            string subject = "CertiTrack – Account Creation Request";
            string body = $@"
                        <p>Hello,</p>
                        <p>A request has been made to create an account for you on <strong>CertiTrack</strong>.</p>
                        <p>Please click the link below to complete your registration. This link will expire in <strong>24 hours</strong>:</p>
                        <p><a href='{activationLink}'>Create Your Account</a></p>
                        <p>If you did not request this, please ignore this message.</p>
                        <p>Thank you,<br/>The CertiTrack Team</p>";

            try
            {
                await _gemailService.SendEmailAsync(model.Email, subject, body);
                ViewBag.Message = "Invitation sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to send invitation email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }

            // If roles are needed again on return
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            ViewBag.Types = await _institutionTypeService.GetAllInstitutionTypesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            var userInvite = await _userRegistrationInviteService.ValidateTokenAsync(token);
            if (userInvite == null || userInvite.Used || userInvite.ExpiresAt < DateTime.UtcNow)
            // if (userInvite == null)
            {
                return NotFound("Invalid or expired token.");
            }
            // Proceed with account activation logic
            // e.g., create a user account, set password, etc.
            // Mark the invite as used
            ViewBag.Roles = await _roleService.GetAllRolesAsync();
            ViewBag.Titles = await _titleService.GetAllTitlesAsync();



            userInvite.Used = true;
            await _userRegistrationInviteService.UpdateInviteAsync(userInvite);

            // Forward to CompleteRegistration GET with email, roleId, etc.
            return RedirectToAction("CompleteRegistration", new
            {
                email = userInvite.Email,
                roleId = userInvite.RoleId,
                token = token,
                typeId = userInvite.TypeId,
                inviteId = userInvite.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> CompleteRegistration(string email, long roleId, string token, long typeId, long inviteId)
        {
            //ViewBag.Roles = await _roleService.GetAllRolesAsync();
            var titles = await _titleService.GetAllTitlesAsync();
            ViewBag.Titles = new SelectList(titles, "Id", "Name");

            var schools = await _schoolService.GetAllSchoolsAsync();
            ViewBag.Schools = new SelectList(schools, "Id", "Name");

            var orgs = await _institutionService.GetAllInstitutionsAsync();
            ViewBag.Institutions = new SelectList(orgs, "Id", "Name");

            var model = new CompleteRegistrationVM
            {
                Email = email,
                RoleId = roleId,
                Token = token,
                InviteId = inviteId,
                TypeId = typeId,
                FirstName = string.Empty,
                LastName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Phone = string.Empty,
                Address = string.Empty,
                Title = 0,
                //OrganizationName = string.Empty,
                OrganizationAddress = string.Empty,
                OrganizationPhone = string.Empty,
                OrganizationEmail = string.Empty,
                RCNo = string.Empty,
                ITNo = string.Empty,
                BNNo = string.Empty,
                SchoolId = 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationVM model)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>])[A-Za-z\d!@#$%^&*(),.?""{}|<>]{8,}$", RegexOptions.IgnoreCase); // Example regex for password validation

            if (!ModelState.IsValid)
            {
                // ViewBag.Roles = await _roleService.GetAllRolesAsync();
                var titles = await _titleService.GetAllTitlesAsync();
                ViewBag.Titles = new SelectList(titles, "Id", "Name");
                ViewBag.Institutions = await _institutionService.GetAllInstitutionsAsync();
                var schools = await _schoolService.GetAllSchoolsAsync();
                ViewBag.Schools = new SelectList(schools, "Id", "Name");
                var orgs = await _institutionService.GetAllInstitutionsAsync();
                ViewBag.Institutions = new SelectList(orgs, "Id", "Name");
                return View(model);
            }

            var existingInvite = await _userRegistrationInviteService.ValidateTokenAsync(model.Token);
            //  existingInvite.TypeId = model.TypeId;
            existingInvite.OrganizationName = model.OrganizationName == null ? string.Empty : model.OrganizationName.ToString();
            existingInvite.OrganizationAddress = model.OrganizationAddress;
            existingInvite.OrganizationPhone = model.OrganizationPhone;
            existingInvite.OrganizationEmail = model.OrganizationEmail;
            existingInvite.RCNo = model.RCNo;
            existingInvite.ITNo = model.ITNo;
            existingInvite.BNNo = model.BNNo;
            existingInvite.SchoolId = model.SchoolId == null ? 0 : model.SchoolId;

            if (existingInvite.RoleId == 3)
            {
                if (model.CoverLetter == null || model.CoverLetter.Length == 0)
                    return BadRequest("Cover Letter file is required.");

                var ext = Path.GetExtension(model.CoverLetter.FileName).ToLowerInvariant();
                var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);

                var originalFilePath = Path.Combine(tempDir, model.CoverLetter.FileName);

                using (var stream = new FileStream(originalFilePath, FileMode.Create))
                {
                    await model.CoverLetter.CopyToAsync(stream);
                }

                byte[] originalBytes = await System.IO.File.ReadAllBytesAsync(originalFilePath);
                byte[] officialPdf;
                string pdfFileName = Path.GetFileNameWithoutExtension(model.CoverLetter.FileName) + ".pdf";

                // Convert to PDF if needed  ".jpg", ".jpeg", ".png", ".bmp", ".gif"
                if (ext == ".pdf")
                {
                    officialPdf = originalBytes;
                }
                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif")
                {
                    officialPdf = FileToPdfConverter.ConvertImageToPdf(originalBytes);
                }
                else if (ext == ".doc" || ext == ".docx")
                {
                    officialPdf = FileToPdfConverter.ConvertDocxToPdf(originalBytes);
                }
                else
                {
                    return BadRequest("Unsupported file type. Only PDF, Word, or image files are allowed.");
                }

                // Save official version
                var officialPath = Path.Combine("wwwroot", "coverLetters", pdfFileName);
                await System.IO.File.WriteAllBytesAsync(officialPath, officialPdf);

                existingInvite.CoverLetterPath = officialPath;
            }

          

            await _userRegistrationInviteService.UpdateInviteAsync(existingInvite);
            ////if (existingInvite == null || existingInvite.Used || existingInvite.ExpiresAt < DateTime.UtcNow)
            //if (existingInvite == null)
            //{
            //    ModelState.AddModelError("", "Invalid or expired invite token.");
            //    var titles = await _titleService.GetAllTitlesAsync();
            //    ViewBag.Titles = new SelectList(titles, "Id", "Name");
            //    return View(model);
            //}

            // Optional: Check if user already exists

            
            var existingUser = await _userService.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "A user with this email already exists.");
                var titles = await _titleService.GetAllTitlesAsync();
                ViewBag.Titles = new SelectList(titles, "Id", "Name");
                var orgs = await _institutionService.GetAllInstitutionsAsync();
                ViewBag.Institutions = new SelectList(orgs, "Id", "Name");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Password) && !passwordRegex.IsMatch(model.Password))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long, contain at least one uppercase letter, one digit, and one special character.");
                var titles = await _titleService.GetAllTitlesAsync();
                ViewBag.Titles = new SelectList(titles, "Id", "Name");
                return View(model);
            }

            //var HashPassword

            // Register user
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = string.Empty,
                TitleId = model.Title,
                RoleId = model.RoleId,
                Created = DateTime.UtcNow,
                PasswordHash = HashPassword(model.Password), // Hash the password
                PhoneNumber = model.Phone,
                Address = model.Address,
                Updated = DateTime.UtcNow,
                CreatedBy = "Self",//HttpContext.Session.GetString("UserEmail"), // Assuming you have a way to get the current user's email
                LastModifiedBy = "Self",//HttpContext.Session.GetString("UserEmail"),
                InviteId = existingInvite.Id,
                ApprovalToken = Guid.NewGuid().ToString()

                // other fields as required
            };

            // await _userService.CreateUserAsync(user, model.Password);

            // Mark the invite as used
            //         existingInvite.Used = true;
            //  await _userRegistrationInviteService.UpdateInviteAsync(existingInvite);

            await _userService.RegisterUserAsync(user);   


            TempData["Message"] = "Account successfully created!";

            await SendApprovalRequestEmailAsync(user); // Send approval request email

            return RedirectToAction("Index", "Home");
        }

        public async Task SendApprovalRequestEmailAsync(User user)
        {
            //Url.Action("ActivateAccount", "Account", new { token = token }, Request.Scheme);
            //var approveUrl = $"{_config["AppBaseUrl"]}/Account/Approve?token={user.ApprovalToken}";
            //var rejectUrl = $"{_config["AppBaseUrl"]}/Account/Reject?token={user.ApprovalToken}";

            var approveUrl = Url.Action("Approve", "Account", new { token = user.ApprovalToken }, Request.Scheme); 
            var rejectUrl = Url.Action("Reject", "Account", new { token = user.ApprovalToken }, Request.Scheme);

            var userInvite = (await _userRegistrationInviteService.GetAlllUserRegistrationInvitesAsync())
            .FirstOrDefault(invite => invite.Id == user.InviteId);

            string subject = "CertiTrack – New Account Request";
            var body = $@"
                <p>A new user has requested an account: <strong>{user.Email}</strong></p>
                <p>Do you want to approve this request?</p>
                <p>
                    <a href='{approveUrl}' style='color:green;'>YES</a> |
                    <a href='{rejectUrl}' style='color:red;'>NO</a>
                </p>";

            try
            {
                await _gemailService.SendEmailAsync(userInvite.OrganizationEmail, subject, body);
                ViewBag.Message = "Invitation sent successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to send invitation email. Please try again.");
                // Optionally log: _logger.LogError(ex, "Email sending failed.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string token)
        {
            var user = (await _userService.GetAllUsersAsync()).FirstOrDefault(u => u.ApprovalToken == token);
            if (user == null) return NotFound();

            user.IsActive = true;
            user.ApprovalToken = string.Empty; // Prevent reuse

            await _userService.UpdateUserAsync(user);

            //return Content("Account approved successfully!");
            ViewBag.Message = "Account approved successfully!";
            return View("ApprovalResult");
        }

        [HttpGet]
        public async Task<IActionResult> Reject(string token)
        {
            var user = (await _userService.GetAllUsersAsync()).FirstOrDefault(u => u.ApprovalToken == token);
            if (user == null) return NotFound();

            user.IsActive = true;
            user.ApprovalToken = string.Empty; // Prevent reuse

            await _userService.UpdateUserAsync(user);
            // return Content("Account Approval Declined!");
            ViewBag.Message = "Account request was rejected.";
            return View("ApprovalResult");
        }



    }
}
