using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qcu_unit_windows.Properties
{
    [FirestoreData]
    public class UploadedSystem
    {
        [FirestoreProperty] public string Id { get; set; } = "";
        [FirestoreProperty] public string Title { get; set; } = "";
        [FirestoreProperty] public string? Description { get; set; }
        [FirestoreProperty] public string? Instructions { get; set; }
        [FirestoreProperty] public string ZipFileName { get; set; } = "";
        [FirestoreProperty] public string? ImageUrl { get; set; }
    }
}