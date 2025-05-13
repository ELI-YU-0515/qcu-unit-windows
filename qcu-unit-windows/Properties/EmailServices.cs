using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

public class EmailServices
{
    private readonly string _apiKey;
    private readonly string _fromEmail = "your-email@example.com"; // Your "from" email
    private readonly string _fromName = "Your Name or Company";

    public EmailServices()
    {
        // Replace with environment variable or configuration
        _apiKey = "your-sendgrid-api-key"; // Your SendGrid API key

        // Optional: Ensure it's not empty
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new ArgumentException("SENDGRID_API_KEY is required but not provided.");
        }
    }

    public async Task<bool> SendVerificationCodeAsync(string recipientEmail, string recipientName, string code)
    {
        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(recipientEmail, recipientName);
            var subject = "Your Verification Code";

            var plainTextContent = $"Hello {recipientName},\n\nYour verification code is: {code}";
            var htmlContent = $"<strong>Hello {recipientName},</strong><br><br>Your verification code is: <h2>{code}</h2><br>Do not share this code with anyone.";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.StatusCode == HttpStatusCode.Accepted;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending verification email: {ex.Message}");
            return false;
        }
    }
}
