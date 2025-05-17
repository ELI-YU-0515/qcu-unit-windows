using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirestoreService
{
    public FirestoreDb _firestore;

    public FirestoreService()
    {
        // Path to your service account JSON key

        string pathToJson = "";

        // Load the credential from the JSON key file
        GoogleCredential credential = GoogleCredential.FromFile(pathToJson);

        // Create Firestore client with the credential
        _firestore = new FirestoreDbBuilder
        {
            ProjectId = "qcu-repo-dbf94",
            Credential = credential
        }.Build();
    }


    // Checks if user is first time by checking 'hasResetPassword' flag (false means first time)
    public async Task<bool> IsFirstTimeUserAsync(string userId)
    {
        var docRef = _firestore.Collection("students").Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists && snapshot.ContainsField("hasResetPassword"))
        {
            bool hasReset = snapshot.GetValue<bool>("hasResetPassword");
            return !hasReset; // true if first-time
        }

        // No field or no doc = treat as first-time
        return true;
    }


    // Mark user as not first time (password reset done)
    public async Task MarkUserAsNotFirstTimeAsync(string userId)
    {
        var docRef = _firestore.Collection("students").Document(userId);
        Dictionary<string, object> updates = new()
    {
        { "hasResetPassword", true }
    };

        await docRef.SetAsync(updates, SetOptions.MergeAll);
    }

}
