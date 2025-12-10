using System.Windows;

namespace ScreenEdgeLight
{
    public partial class App : Application
    {
        public static OverlayWindow Overlay { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create and show overlay
            OverlayWindow overlay = new OverlayWindow();
            Overlay = overlay;
            overlay.Show();

            // Create and show the control window
            MainWindow main = new MainWindow();
            MainWindow = main;  // let WPF know this is the main window
            main.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            main.Show();
            main.Activate();
        }
    }
}
