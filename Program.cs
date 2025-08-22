using Certitrack.Data;
using Certitrack.Repositories;
using Certitrack.Services;
using Certitrack.Services.Certitrack.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;




var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // ? Add this if missing

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITitleRepository, TitleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInstitutionTypeRepository, InstitutionTypeRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IInstitutionTypeService, InstitutionTypeService>();


builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<GEmailService>();

builder.Services.AddScoped<IUserRegistrationInviteRepository, UserRegistrationInviteRepository>();
builder.Services.AddScoped<IUserRegistrationInviteService, UserRegistrationInviteService>();

builder.Services.AddScoped<ICertificateDetailRepository, CertificateDetailRepository>();
builder.Services.AddScoped<ICertificateDetailService, CertificateDetailService>();

builder.Services.AddScoped<IInstitutionRepository, InstitutionRepository>();
builder.Services.AddScoped<IIInstitutionService, InstitutionService> ();

builder.Services.AddScoped<IInstitutionRepository, InstitutionRepository>();
builder.Services.AddScoped<IIInstitutionService, InstitutionService>();

builder.Services.AddScoped<IQualificationTypesRepository,QualificationTypeRepository> ();
builder.Services.AddScoped <IQualificationTypeService,QualificationTypeService> ();

builder.Services.AddScoped<IQualificationClasssRepository, QualificationClassRepository>();
builder.Services.AddScoped<IQualificationClassService, QualificationClassService>();

builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IFacultyService, FacultyService>();

builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<ISchoolService, SchoolService>();

builder.Services.AddScoped<ITranscriptRequestRepository, TranscriptRequestRepository>();
builder.Services.AddScoped<ITranscriptRequestService, TranscriptRequestService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<ISchoolTypeRepository, SchoolTypeRepository>();
builder.Services.AddScoped<ISchoolTypeService, SchoolTypeService>();




builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Set EPPlus License (for version 8+)
//ExcelPackage.License = new EPPlusLicense(LicenseType.NonCommercial);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
