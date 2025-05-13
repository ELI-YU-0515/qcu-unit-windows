using qcu_unit_windows.Properties;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class FirebaseAuthService
{
    private readonly string apiKey = "your-api-key"; // Your Firebase API Key
    private readonly HttpClient httpClient = new();

    public async Task<FirebaseLoginResponse?> SignInWithEmailPassword(string email, string password)
    {
        var request = new
        {
            email = email,
            password = password,
            returnSecureToken = true
        };

        var response = await httpClient.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
            request
        );

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);

            var idToken = jsonDoc.RootElement.GetProperty("idToken").GetString() ?? string.Empty;
            var localId = jsonDoc.RootElement.GetProperty("localId").GetString() ?? string.Empty;

            return new FirebaseLoginResponse
            {
                IdToken = idToken,
                LocalId = localId
            };
        }
        else
        {
            Console.WriteLine($"Login failed: {await response.Content.ReadAsStringAsync()}");
            return null;
        }
    }
}
