using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class MailGunEmailSender : IEmailSender
{
    private readonly ILogger<MailGunEmailSender> _logger;
    private readonly IConfiguration _configuration;

    public MailGunEmailSender(ILogger<MailGunEmailSender> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var apiKey = Environment.GetEnvironmentVariable("MailgunSendingAPIKey");
        var domain = "sandbox07ef598a4afa40728f0c60859ee8a6f5.mailgun.org";

        if (message.Contains("amp;"))
        {
            message = message.Replace("amp;", "");
        }

        using var client = new HttpClient();

        // Basic Auth: "api" as username, API key as password
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        // Form data
        var form = new MultipartFormDataContent
        {
            { new StringContent($"Mailgun Sandbox <postmaster@{domain}>"), "from" },
            { new StringContent(toEmail), "to" },
            { new StringContent(subject), "subject" },
            { new StringContent(message), "text" }
        };

        var url = $"https://api.mailgun.net/v3/{domain}/messages";
        var response = await client.PostAsync(url, form);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent successfully!");
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to send email: {response.StatusCode}");
            _logger.LogError(content);
        }
    }
}