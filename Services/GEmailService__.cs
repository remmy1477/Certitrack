namespace Certitrack.Services
{
    using System.Net;
    using System.Net.Mail;

    namespace Certitrack.Services
    {
        public class GEmailService__
        {
            private readonly IConfiguration _config;

            public GEmailService__(IConfiguration config)
            {
                _config = config;
            }

            public async Task SendEmailAsync(string to, string subject, string body)
            {
                string host = _config["Smtp:Host"];
                string fallbackIp = _config["Smtp:FallbackIp"];
                int port = int.Parse(_config["Smtp:Port"]);
                string username = _config["Smtp:Username"];
                string password = _config["Smtp:Password"];
                bool enableSsl = bool.Parse(_config["Smtp:EnableSsl"]);
                int timeout = int.Parse(_config["Smtp:Timeout"]);

                try
                {
                    await TrySendEmailAsync(host, port, username, password, enableSsl, timeout, to, subject, body, null);
                }
                catch (SmtpException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    Console.WriteLine($"⚠️ DNS failed for {host}, retrying with fallback IP ({fallbackIp})...");
                    await TrySendEmailAsync(fallbackIp, port, username, password, enableSsl, timeout, to, subject, body, null);
                }
            }

            public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath)
            {
                string host = _config["Smtp:Host"];
                string fallbackIp = _config["Smtp:FallbackIp"];
                int port = int.Parse(_config["Smtp:Port"]);
                string username = _config["Smtp:Username"];
                string password = _config["Smtp:Password"];
                bool enableSsl = bool.Parse(_config["Smtp:EnableSsl"]);
                int timeout = int.Parse(_config["Smtp:Timeout"]);

                try
                {
                    await TrySendEmailAsync(host, port, username, password, enableSsl, timeout, to, subject, body, attachmentPath);
                }
                catch (SmtpException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    Console.WriteLine($"⚠️ DNS failed for {host}, retrying with fallback IP ({fallbackIp})...");
                    await TrySendEmailAsync(fallbackIp, port, username, password, enableSsl, timeout, to, subject, body, attachmentPath);
                }
            }

            private async Task TrySendEmailAsync(
                string host, int port, string username, string password, bool enableSsl, int timeout,
                string to, string subject, string body, string? attachmentPath)
            {
                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl,
                    Timeout = timeout // ⏱ set timeout
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    using var attachment = new Attachment(attachmentPath);
                    mailMessage.Attachments.Add(attachment);
                    await smtpClient.SendMailAsync(mailMessage);
                }
                else
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }

                Console.WriteLine($"✅ Email sent successfully via {host}:{port}");
            }
        }
    }

}
