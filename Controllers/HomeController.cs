using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Certitrack.Models;
using Certitrack.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Certitrack.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoleService _roleService;
    private readonly IInstitutionTypeService _institutionTypeService;
    private readonly IIInstitutionService _institutionService;
    private readonly GEmailService _gemailService;
    private readonly IUserRegistrationInviteService _userRegistrationInviteService;

    public HomeController(ILogger<HomeController> logger, IRoleService roleService,
        IInstitutionTypeService institutionTypeService,IIInstitutionService institutionService,
        GEmailService gemailService, IUserRegistrationInviteService userRegistrationInviteService)
    {
        _logger = logger;
        _roleService = roleService;
        _institutionTypeService = institutionTypeService;
        _institutionService = institutionService;
        _gemailService = gemailService;
        _userRegistrationInviteService = userRegistrationInviteService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult CreateUser()
    {
        return View();
    }
    public IActionResult UploadCredential()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> GetRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();

        var roleList = roles.Where(role => role.Id != 1 && role.Id != 2)
                        .Select(role => new SelectListItem
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        })
                        .ToList();

        return Json(roleList);
       
    }

    [HttpGet]
    public async Task<JsonResult> GetOrganizationTypes()
    {
        var organizationTypes = await _institutionTypeService.GetAllInstitutionTypesAsync();

        var organizationTypeList = organizationTypes.Select(role => new SelectListItem
        {
            Value = role.Id.ToString(), // Ensure Value is a string
            Text = role.Name
        }).ToList();

        return Json(organizationTypeList);
    }

    [HttpPost]
    public async Task<IActionResult> SelfService(string email,string role,string organizationType)
    {
        var FreeEmailRegex = new Regex(@"@(gmail|yahoo|outlook|hotmail|live|aol|icloud|mail|protonmail|zoho)\.(com|net|org|co\.uk|ca|in)$", RegexOptions.IgnoreCase);
        var ValidEmailFormatRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$");

        //if (!ModelState.IsValid)
        //    return View(model);

        // var emailDomain = model.Email.Split('@').Last().ToLower();
        if (role != "7")
        {
            bool isFreeEmail = FreeEmailRegex.IsMatch(email);
            if (isFreeEmail)
            {
                var domain = email.Split('@').Last().ToLower();
                return BadRequest(new { success = false, message = $"Emails from '{domain}' are not allowed. Please use your organization's email address." });
            }
        }
        

        if (!ValidEmailFormatRegex.IsMatch(email))
        {
            return BadRequest(new { success = false, message = "Invalid email format." });
        }

        // Save user invite logic here (if applicable)
        // e.g., create a user with a generated token, role, etc.

        // Example token generation
        string token = Guid.NewGuid().ToString(); // store this with the user/invite for later verification
        string activationLink = Url.Action("ActivateAccount", "Account", new { token = token }, Request.Scheme);

        var userRegistrationInvite = new UserRegistrationInvite
        {

            RoleId = long.Parse(role),
            Email = email,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24), // Set expiration time
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            CreatedBy = email, // Assuming you have a way to get the current user's email
            LastModifiedBy = email,
            TypeId = long.Parse(organizationType),
            activationlink = activationLink



        };

        await _userRegistrationInviteService.CreateInviteAsync(userRegistrationInvite);


       // string activationLink = Url.Action("ActivateAccount", "Account", new { token = token }, Request.Scheme);

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
            await _gemailService.SendEmailAsync(email, subject, body);
           //ViewBag.Message = "Invitation sent successfully.";
            return Ok(new { success = true, message = $"Invitation sent successfully." });
        }
        catch (Exception ex)
        {
            //ModelState.AddModelError(string.Empty, "Failed to send invitation email. Please try again.");
            return BadRequest(new { success = false, message = "Failed to send invitation email. Please try again." });
            // Optionally log: _logger.LogError(ex, "Email sending failed.");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
