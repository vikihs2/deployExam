using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace ManagingAgriculture.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration config, ILogger<HomeController> logger)
        {
            _config = config;
            _logger = logger;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "AgroCore - Agriculture Management";
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact - AgroCore";
            ViewBag.SmtpConfigured = !string.IsNullOrWhiteSpace(_config["Smtp:Host"]);
            ViewBag.SmtpHost = _config["Smtp:Host"] ?? string.Empty;
            return View(new Models.ContactForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Models.ContactForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var smtpHost = _config["Smtp:Host"];

            // If SMTP is configured, attempt to send, otherwise log and return success message.
            if (!string.IsNullOrWhiteSpace(smtpHost))
            {
                try
                {
                    var smtpPort = int.TryParse(_config["Smtp:Port"], out var p) ? p : 25;
                    var enableSsl = bool.TryParse(_config["Smtp:EnableSsl"], out var s) ? s : false;

                    var fromAddress = _config["Smtp:From"] ?? "no-reply@agrocore.local";
                    var toAddress = _config["Smtp:To"] ?? "victor.stefanov.highschool@buditel.bg";

                    var msg = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = $"Contact form: {form.FullName}",
                        Body = $"Name: {form.FullName}\nEmail: {form.Email}\n\nMessage:\n{form.Message}",
                        IsBodyHtml = false
                    };

                    using var client = new SmtpClient(smtpHost, smtpPort)
                    {
                        EnableSsl = enableSsl
                    };

                    var user = _config["Smtp:Username"];
                    var pass = _config["Smtp:Password"];
                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        client.Credentials = new NetworkCredential(user, pass);
                    }

                    client.Send(msg);
                    TempData["ContactSuccess"] = "Thanks for your message — we've sent it to our support team.";
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Failed to send contact email");
                    TempData["ContactError"] = "There was an error sending your message. We've recorded it and will follow up shortly.";
                }
            }
            else
            {
                _logger.LogInformation("SMTP not configured - skipping send. Contact from {Email}: {Name}", form.Email, form.FullName);
                TempData["ContactSuccess"] = "Thanks for your message — we will be in touch soon.";
            }

            return RedirectToAction("Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TestSmtp()
        {
            var smtpHost = _config["Smtp:Host"];
            if (string.IsNullOrWhiteSpace(smtpHost))
            {
                TempData["ContactError"] = "SMTP is not configured on this server.";
                return RedirectToAction("Contact");
            }

            try
            {
                var fromAddress = _config["Smtp:From"] ?? "no-reply@agrocore.local";
                var toAddress = _config["Smtp:To"] ?? "victor.stefanov.highschool@buditel.bg";
                var msg = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "AgroCore SMTP test",
                    Body = "This is a test email from AgroCore contact form SMTP check.",
                    IsBodyHtml = false
                };

                var smtpPort = int.TryParse(_config["Smtp:Port"], out var p) ? p : 25;
                var enableSsl = bool.TryParse(_config["Smtp:EnableSsl"], out var s) ? s : false;
                using var client = new SmtpClient(smtpHost, smtpPort) { EnableSsl = enableSsl };
                var user = _config["Smtp:Username"];
                var pass = _config["Smtp:Password"];
                if (!string.IsNullOrWhiteSpace(user)) client.Credentials = new NetworkCredential(user, pass);
                client.Send(msg);
                TempData["ContactSuccess"] = "Test email sent successfully.";
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "SMTP test failed");
                TempData["ContactError"] = "SMTP test failed — check logs and configuration.";
            }

            return RedirectToAction("Contact");
        }
    }
}
