using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

public class EmailServices
{
    private readonly string _apiKey = ""; // Remove hardcoded API key
    private readonly string _fromEmail = "qcu.repo@gmail.com";
    private readonly string _fromName = "QCU Repository";

    public async Task<bool> SendVerificationCodeAsync(string recipientEmail, string recipientName, string code)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(recipientEmail, recipientName);
        var subject = "Your QCU Verification Code";
        var plainTextContent = $"Hello {recipientName},\n\nYour verification code is: {code}";
        var htmlContent = $"<strong>Hello {recipientName},</strong><br><br>Your verification code is: <h2>{code}</h2><br>Do not share this code.";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        // Log response status code and body for debugging
        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            string responseBody = await response.Body.ReadAsStringAsync();
            Console.WriteLine($"Email sending failed: {response.StatusCode} - {responseBody}");
            return false;
        }

        return true;
    }
}