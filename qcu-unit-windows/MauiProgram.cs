using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Extensions.Logging;

namespace qcu_unit_windows
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Register FirebaseAuthClient
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyDFhJ1SBO3M2AFstZyz50fwXdx57POB3N0",
                AuthDomain = "qcu-repo-dbf94.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            }));

            // ✅ Register your custom FirebaseAuthService
            builder.Services.AddSingleton<FirebaseAuthService>();
            builder.Services.AddSingleton<EmailServices>();
            builder.Services.AddSingleton<FirestoreService>();



            return builder.Build();
        }
    }
}