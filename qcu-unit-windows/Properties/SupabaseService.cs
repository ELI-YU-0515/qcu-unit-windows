using Supabase;
using Supabase.Storage;
using System;
using System.Threading.Tasks;

public class SupabaseService
{
    private Supabase.Client _client;

    // Initialize Supabase client asynchronously
    public async Task InitializeAsync()
    {
        _client = new Supabase.Client(
            "",
            ""        
            );
        await _client.InitializeAsync();
    }

    // Upload ZIP file to specified bucket
    public async Task<string> UploadZipFileAsync(string bucketName, string filePath, byte[] fileBytes)
    {
        if (_client == null)
            throw new InvalidOperationException("Supabase client not initialized. Call InitializeAsync first.");

        var bucket = _client.Storage.From(bucketName);

        // Upload expects (byte[] file, string path)
        await bucket.Upload(fileBytes, filePath);

        // Get public URL of the uploaded file
        string publicUrl = bucket.GetPublicUrl(filePath);

        return publicUrl;
    }
}
