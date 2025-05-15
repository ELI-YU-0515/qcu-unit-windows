using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

public class EmailServices
{
    private readonly string _apiKey;
    private readonly string _fromEmail = "qcu.repo@gmail.com";
    private readonly string _fromName = "QCU REPOSITORY";

    // Simple in-memory storage for verification codes (email -> code)
    private static ConcurrentDictionary<string, string> verificationCodes = new();

    public EmailServices()
    {
        _apiKey = "";

        if (string.IsNullOrEmpty(_apiKey))
            throw new ArgumentException("SENDGRID_API_KEY is required but not provided.");
    }

    public async Task<bool> SendVerificationCodeAsync(string recipientEmail)
    {
        // Generate 6-digit code
        var code = new Random().Next(100000, 999999).ToString();

        // Store the code
        verificationCodes[recipientEmail] = code;

        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(recipientEmail);
            var subject = "Your Verification Code";

            var plainTextContent = $"Hello,\n\nYour verification code is: {code}";
            var htmlContent = $"<strong>Hello,</strong><br><br>Your verification code is: <h2>{code}</h2><br>Do not share this code with anyone.";

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

    public Task<bool> VerifyCodeAsync(string email, string code)
    {
        if (verificationCodes.TryGetValue(email, out var storedCode))
        {
            if (storedCode == code)
            {
                // Remove code after successful verification
                verificationCodes.TryRemove(email, out _);
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
    }
}
