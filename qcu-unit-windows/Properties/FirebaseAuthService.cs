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

    private string? currentEmail;
    private string? currentIdToken;

    public string? GetCurrentEmail() => currentEmail;
    public string? GetIdToken() => currentIdToken;

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
                currentEmail = jsonDoc.RootElement.GetProperty("email").GetString(); // ✅ store email
                currentEmail = email; // ← Add this line

                return new FirebaseLoginResult
                {
                    IsSuccess = true,
                    IdToken = currentIdToken ?? string.Empty,
                    LocalId = jsonDoc.RootElement.GetProperty("localId").GetString() ?? string.Empty
                };
            }
            else
            {
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

    public async Task<string> SendPasswordResetEmailAsync(string email)
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

        var responseBody = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode
            ? "Success"
            : $"Error: {responseBody}";
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

    public async Task LogoutAsync()
    {
        currentIdToken = null;
        currentEmail = null; // ✅ reset email on logout
        await Task.CompletedTask;
    }

    public async Task<FirebaseLoginResult> RegisterUserAsync(string email, string password)
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
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}",
                request);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonDoc = JsonDocument.Parse(content);

                currentIdToken = jsonDoc.RootElement.GetProperty("idToken").GetString();
                currentEmail = email;

                return new FirebaseLoginResult
                {
                    IsSuccess = true,
                    IdToken = currentIdToken ?? string.Empty,
                    LocalId = jsonDoc.RootElement.GetProperty("localId").GetString() ?? string.Empty
                };
            }
            else
            {
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

}

