using Certitrack.Models;
using Certitrack.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Certitrack.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICertificateDetailService _CDService;
        private readonly IPaymentService _paymentService;
        private readonly ITranscriptRequestService _transcriptRequestService;
        public PaymentController(IHttpClientFactory httpClientFactory, ICertificateDetailService CDService,
            IPaymentService paymentService, ITranscriptRequestService transcriptRequestService)
        {
            _httpClientFactory = httpClientFactory;
            _CDService = CDService;
            _paymentService = paymentService;
            _transcriptRequestService = transcriptRequestService;
        }
        public IActionResult Index()
        {
            return View();
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
