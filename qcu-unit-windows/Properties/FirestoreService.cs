using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FirestoreService
{
    private readonly HttpClient _httpClient = new();
    private readonly string _projectId = "your-project-id"; // Your project ID

    public async Task<bool> UpdateUserPasswordAsync(string uid, string idToken, string newPassword)
    {
        try
        {
            // Firestore document path for the user
            var docPath = $"projects/{_projectId}/databases/(default)/documents/users/{uid}";

            // Define the patch data with fields to update
            var patchData = new
            {
                fields = new Dictionary<string, object>
                {
                    { "permanent_password", new { stringValue = newPassword } },
                    { "has_permanent_password", new { booleanValue = true } }
                }
            };

            // Create the HTTP request for patching the document
            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://firestore.googleapis.com/v1/{docPath}")
            {
                Content = JsonContent.Create(patchData)
            };

            // Set the authorization token (ID token)
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

            // Send the request
            var response = await _httpClient.SendAsync(request);

            // Check the response status
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Password updated successfully via REST.");
                return true;
            }

            // Handle failure case
            Console.WriteLine($"Failed to update password: {await response.Content.ReadAsStringAsync()}");
            return false;
        }
        catch (Exception ex)
        {
            // Catch exceptions and log error
            Console.WriteLine($"Exception updating Firestore: {ex.Message}");
            return false;
        }
    }
}