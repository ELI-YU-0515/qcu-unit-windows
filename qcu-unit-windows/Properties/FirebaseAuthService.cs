using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using qcu_unit_windows.Models;

public class FirebaseLoginResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = "";
    public string IdToken { get; set; } = "";
    public string LocalId { get; set; } = "";
}

public class FirebaseAuthService
{
    private readonly string apiKey = "AIzaSyDFhJ1SBO3M2AFstZyz50fwXdx57POB3N0";
    private static readonly HttpClient httpClient = new();

    private string? currentIdToken;

    public async Task<FirebaseLoginResult> LoginAsync(string email, string password)
    {
        var request = new
        {
            email,
            password,
            returnSecureToken = true
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                request);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonDoc = JsonDocument.Parse(content);

                currentIdToken = jsonDoc.RootElement.GetProperty("idToken").GetString();

                return new FirebaseLoginResult
                {
                    IsSuccess = true,
                    IdToken = currentIdToken ?? string.Empty,
                    LocalId = jsonDoc.RootElement.GetProperty("localId").GetString() ?? string.Empty
                };
            }
            else
            {
                // Parse error message
                var errorMessage = "Unknown error";

                try
                {
                    var jsonDoc = JsonDocument.Parse(content);
                    errorMessage = jsonDoc.RootElement.GetProperty("error").GetProperty("message").GetString() ?? errorMessage;
                }
                catch { }

                return new FirebaseLoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage
                };
            }
        }
        catch (Exception ex)
        {
            return new FirebaseLoginResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public string? GetIdToken()
    {
        return currentIdToken;
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email)
    {
        var request = new
        {
            requestType = "PASSWORD_RESET",
            email = email
        };

        var response = await httpClient.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={apiKey}",
            request
        );

        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        currentIdToken = null;
        await Task.CompletedTask;
    }

    public async Task<bool> UpdatePasswordAsync(string idToken, string newPassword)
    {
        var request = new
        {
            idToken = idToken,
            password = newPassword,
            returnSecureToken = true
        };

        var response = await httpClient.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:update?key={apiKey}",
            request
        );

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            currentIdToken = jsonDoc.RootElement.GetProperty("idToken").GetString();
            return true;
        }
        else
        {
            return false;
        }
    }
}
