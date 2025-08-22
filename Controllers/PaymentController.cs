using Certitrack.Models;
using Certitrack.Services;
using Certitrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using X.PagedList.Extensions;

namespace Certitrack.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateDetailService _CDService;
        private readonly IPaymentService _paymentService;
        private readonly ITranscriptRequestService _transcriptRequestService;

        private readonly IInstitutionTypeService _institutionTypeService;
        private readonly IDepartmentService _departmentService;
        private readonly ISchoolTypeService _schoolTypeService;
        private readonly ISchoolService _schoolService;
        private readonly IIInstitutionService _institutionService;
        private readonly IFacultyService _faculyService;
        private readonly IQualificationClassService _qualificationClassService;
        private readonly IQualificationTypeService _qualificationTypeService;
        public PaymentController(IHttpClientFactory httpClientFactory, ICertificateDetailService CDService,
            IPaymentService paymentService, ITranscriptRequestService transcriptRequestService,
            IInstitutionTypeService institutionTypeService, IDepartmentService departmentService,
            ISchoolTypeService schoolTypeService, ISchoolService schoolService, IIInstitutionService institutionService,
            IFacultyService faculyService, IQualificationClassService qualificationClassService, IQualificationTypeService qualificationTypeService)
        {
            _httpClientFactory = httpClientFactory;
            _CDService = CDService;
            _paymentService = paymentService;
            _transcriptRequestService = transcriptRequestService;
            _institutionTypeService = institutionTypeService;
            _departmentService = departmentService;
            _schoolService = schoolService;
            _institutionService = institutionService;
            _schoolTypeService = schoolTypeService;
            _faculyService = faculyService;
            _qualificationClassService = qualificationClassService;
            _qualificationTypeService = qualificationTypeService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = page ?? 1;

                var allpayments = await _paymentService.GetAllPaymentAsync();

               

                    //IEnumerable<CertificateDetail> credentials;


                string institutionType = HttpContext?.Session?.GetString("InstitutionType") ?? string.Empty;
                string schoolId = HttpContext?.Session?.GetString("SchoolId") ?? "0";
                string institutionId = HttpContext?.Session?.GetString("InstitutionId") ?? "0";
                string roleName = HttpContext?.Session?.GetString("UserRoleName");
                var UserEmail = HttpContext?.Session?.GetString("UserEmail");

                //IEnumerable<Payment> payments = Enumerable.Empty<Payment>();
                //List<Payment> payments = new List<Payment>();

                //if (roleName == "SuperAdmin" || roleName == "AdminUser")
                //{
                //    payments = allpayments.ToList();

                //}

                //foreach (var payment in allpayments)
                //{
                //    if (payment.PaymentType == "C")
                //    {
                //        var cd = await _CDService.GetCertificateDetailByIdAsync(payment.TypeId);
                //        if(cd.SchoolId == long.Parse(schoolId))
                //        {
                //            payments.add(payment);
                //        }

                //    }

                //}

                //else if (roleName == "Student" || roleName == "Agent")
                //{
                //    credentials = allCredentials.Where(cr => cr.CreatedBy == UserEmail);
                //}
                //else if (schoolId != "0")
                //{
                //    //var schCedentials = await _CDService.GetAllCertificateDetailAsync();
                //    credentials = allCredentials.Where(cr => cr.SchoolId == long.Parse(schoolId));
                //}
                //else if (institutionId != "0")
                //{
                //    //var instCredentials = await _CDService.GetAllCertificateDetailAsync();
                //    credentials = allCredentials.Where(cr => cr.InstitutionId == long.Parse(institutionId));
                //}

                List<Payment> payments = new List<Payment>();

                if (roleName == "SuperAdmin" || roleName == "AdminUser")
                {
                    payments = allpayments.ToList();
                }
                else
                {
                    foreach (var payment in allpayments)
                    {
                        if (payment.PaymentType == "C")
                        {
                            var cd = await _CDService.GetCertificateDetailByIdAsync(payment.TypeId);
                            if (cd != null && cd.SchoolId == long.Parse(schoolId))
                            {
                                payments.Add(payment);
                            }
                        }
                        else if (payment.PaymentType == "T")
                        {
                            var td = await _transcriptRequestService.GetTranscriptRequestByIdAsync(payment.TypeId);
                            if (td != null && td.SchoolId == long.Parse(schoolId))
                            {
                                payments.Add(payment);
                            }
                        }
                    }
                }


                var paymentsList = new List<PaymentVM>();

                if (payments == null || !payments.Any())
                {
                    TempData["Message"] = "No Payment Record Detail found.";
                    //var pagedList = creditialsList
                    return View(paymentsList.ToList().ToPagedList());
                }
                else
                {
                    foreach (var payment in payments)
                    {
                        //var school = await _schoolService.GetSchoolByIdAsync(credential.SchoolId);
                        //var institution = await _institutionService.GetInstitutionByIdAsync(credential.InstitutionId);
                        //var faculty = await _faculyService.GetFacultyByIdAsync(credential.FacultyId);
                        //var qualificationType = await _qualificationTypeService.GetQualificationTypeByIdAsync(credential.DegreeId);
                        //var qualificationClass = await _qualificationClassService.GetQualificationClassByIdAsync(credential.DegreeClassId);
                        var pt = payment.PaymentType;
                        if (pt == "C")
                        {
                           var cd = await _CDService.GetCertificateDetailByIdAsync(payment.TypeId);
                            payment.FullName = $"{cd.HolderFirstName} {cd.HolderMiddleName} {cd.HolderLastName}";

                        }
                        else 
                        {
                            var tr = await _transcriptRequestService.GetTranscriptRequestByIdAsync(payment.TypeId); 
                            payment.FullName = $"{tr.FirstName} {tr.MiddleName} {tr.LastName}";
                        }
                            paymentsList.Add(new PaymentVM
                            {
                                Id = payment.Id,
                                Reference = payment.Reference,
                                Email = payment.Email,
                                FullName = payment.FullName,
                                Amount = payment.Amount,
                                Paid = payment.Paid,
                                PaymentChannel = payment.PaymentChannel,
                                GatewayResponse = payment.GatewayResponse,
                                Currency = payment.Currency,
                                TransactionDate = payment.TransactionDate,
                                PaymentType = payment.PaymentType,

                            });
                    }
                }

                var pagedList = paymentsList.OrderBy(u => u.Id).ToList().ToPagedList(pageNumber, pageSize);




                return View(pagedList);
            }
            catch (Exception ex)
            {

                return View(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Initialize(long id, string email, int amount,string name,string type)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_56e79644b0f85801b5f166d5c7e63aba15219127"); // Replace with your secret key

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email = email,
                amount = amount,
                name = name,    
                callback_url = Url.Action("Verify", "Payment", new { id,type }, Request.Scheme)
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.paystack.co/transaction/initialize", content);
            var result = await response.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }

        public async Task<IActionResult> Verify(string reference, long id,string type)
        {

            try 
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_56e79644b0f85801b5f166d5c7e63aba15219127");

                var response = await client.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");
                var resultContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(resultContent);

                if (result.status == true && result.data.status == "success")
                {
                    // Mark record as paid
                    var data = result["data"];
                    decimal amount = 0;
                    decimal.TryParse(data["amount"]?.ToString(), out amount);

                    //var tDate = DateTime.TryParse(data["transaction_date"]?.ToString(), out var parsedDate)
                    //            ? parsedDate
                    //            : DateTime.MinValue; // or DateTime.UtcNow or throw, depending on your logic
                    // Amount = amount;
                    var payment = new Payment
                    {
                        Reference = data["reference"]?.ToString(),
                        Email = data["customer"]?["email"]?.ToString(),
                        FullName = data["customer"]?["first_name"] + " " + data["customer"]?["last_name"],
                        Amount = amount,
                        Paid = data["status"]?.ToString() == "success" ? true : false,
                        PaymentChannel = data["channel"]?.ToString(),
                        GatewayResponse = data["gateway_response"]?.ToString(),
                        Currency = data["currency"]?.ToString(),
                        TransactionDate = DateTime.Parse(data["transaction_date"]?.ToString()),
                        PaymentType = type, // optional
                        TypeId = id, // Assuming TypeId is the ID of the credential or transcript
                        CreatedBy = HttpContext.Session.GetString("UserEmail") ?? "System",
                        LastModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "System"
                    };
                    // Save payment record to the database
                    var paymentResult = await _paymentService.InsertPaymentAsync(payment);
                    //if (paymentResult == "error")
                    //{
                    //    TempData["Message"] = "Error processing payment.";
                    //    return RedirectToAction("Index", "Credential");
                    //}

                    // {
                    if (type == "C")
                    {
                        var cred = await _CDService.GetCertificateDetailByIdAsync(id);
                        cred.Ispaid = true;
                        await _CDService.UpdateCredenttialDetailAsync(cred);
                    }
                    else if (type == "T")
                    {
                        var trans = await _transcriptRequestService.GetTranscriptRequestByIdAsync(id);
                        trans.IsPaid = true;
                        await _transcriptRequestService.UpdateTranscriptRequestAsync(trans);
                        // Handle transcript payment logic here
                        // For example, update the transcript record as paid
                    }


                    TempData["Message"] = "Payment successful.";
                }
                else
                {
                    TempData["Message"] = "Payment failed or cancelled.";
                }

                if (type == "C")

                    return RedirectToAction("Index", "Credential");
                else
                    return RedirectToAction("Index", "Transcript");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error processing payment: " + ex.Message;
                return RedirectToAction("Index", "Credential");
            }   
            
        }


    }
}
