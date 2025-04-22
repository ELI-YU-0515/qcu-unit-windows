using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;
using WinRT.Interop;

namespace qcu_unit_windows.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp()
        {
            var mauiApp = MauiProgram.CreateMauiApp();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("WindowMaximize", (handler, view) =>
            {
#if WINDOWS
                var nativeWindow = handler.PlatformView;
                var windowHandle = WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = AppWindow.GetFromWindowId(windowId);

                // Use OverlappedPresenter to keep default window controls
                var presenter = appWindow.Presenter as OverlappedPresenter;
                if (presenter != null)
                {
                    presenter.Maximize(); // ⬛ Maximize the window
                }
#endif
            });

            return mauiApp;
        }
    }
}
