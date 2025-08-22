using System.Net.Mail;
using System.Net;

namespace Certitrack.Services
{
    public class GEmailService
    {
        private readonly IConfiguration _config;

        public GEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = bool.Parse(_config["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:Username"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        //public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
        //{
        //    try 
        //    {
        //        var smtpClient = new SmtpClient(_config["Smtp:Host"])
        //        {
        //            Port = int.Parse(_config["Smtp:Port"]),
        //            Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
        //            EnableSsl = bool.Parse(_config["Smtp:EnableSsl"])
        //        };
        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(_config["Smtp:Username"]),
        //            Subject = subject,
        //            Body = body,
        //            IsBodyHtml = true,
        //        };
        //        mailMessage.To.Add(to);
        //        mailMessage.Attachments.Add(new Attachment(attachmentPath));
        //        await smtpClient.SendMailAsync(mailMessage);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error : {ex.Message}");
        //        throw;
        //    }
        //}

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"])))
                {
                    smtpClient.Credentials = new NetworkCredential(
                        _config["Smtp:Username"],
                        _config["Smtp:Password"]
                    );
                    smtpClient.EnableSsl = bool.Parse(_config["Smtp:EnableSsl"]);

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_config["Smtp:Username"]);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.To.Add(to);

                        using (var attachment = new Attachment(attachmentPath))
                        {
                            mailMessage.Attachments.Add(attachment);

                            await smtpClient.SendMailAsync(mailMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
