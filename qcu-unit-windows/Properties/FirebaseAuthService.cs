using Firebase.Auth.Providers;
using Firebase.Auth;

public class FirebaseAuthService
{
    private readonly FirebaseAuthClient _authClient;

    public FirebaseAuthService()
    {
        var config = new FirebaseAuthConfig
        {
            ApiKey = "AIzaSyDFhJ1SBO3M2AFstZyz50fwXdx57POB3N0",
            AuthDomain = "qcu-repo-dbf94.firebaseapp.com",
            Providers = new FirebaseAuthProvider[] { new EmailProvider() }
        };

        _authClient = new FirebaseAuthClient(config);
    }

    public async Task<string> LoginUser(string email, string password)
    {
        try
        {
            var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            return userCredential.User.Credential.IdToken;
        }
        catch (Exception ex)
        {
            throw new Exception($"Login failed: {ex.Message}");
        }
    }
}