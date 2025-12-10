using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WinForms = System.Windows.Forms;
using Drawing = System.Drawing;

namespace ScreenEdgeLight
{
    public partial class MainWindow : Window
    {
        private OverlayWindow Overlay
        {
            get { return App.Overlay; }
        }

        private WinForms.NotifyIcon _trayIcon;

        public MainWindow()
        {
            InitializeComponent();

            // Create tray icon
            _trayIcon = new WinForms.NotifyIcon();
            _trayIcon.Icon = Drawing.SystemIcons.Application; // you can swap this for your own .ico
            _trayIcon.Text = "Screen Edge Light";
            _trayIcon.Visible = false;

            // Tray context menu
            WinForms.ContextMenuStrip menu = new WinForms.ContextMenuStrip();
            menu.Items.Add("Show", null, OnTrayShowClicked);
            menu.Items.Add("Exit", null, OnTrayExitClicked);
            _trayIcon.ContextMenuStrip = menu;

            // Double-click tray icon to show window
            _trayIcon.MouseUp += TrayIcon_MouseUp;

            // Optional hotkey: Ctrl+Shift+E to toggle this window
            RoutedCommand toggleCmd = new RoutedCommand();
            toggleCmd.InputGestures.Add(
                new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift));
            CommandBindings.Add(new CommandBinding(toggleCmd, ToggleSelf));

            Loaded += MainWindow_Loaded;
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOverlayEnabled();
            UpdateOverlayThickness();
            UpdateOverlayOpacity();
            SetWarmColor();
        }

        // ------------ Tray handling ------------

        private void MinimizeToTray_Click(object sender, RoutedEventArgs e)
        {
            MinimizeToTray();
        }
        private void TrayIcon_MouseUp(object sender, WinForms.MouseEventArgs e)
{
    if (e.Button == WinForms.MouseButtons.Left)
    {
        if (Visibility == Visibility.Visible)
    MinimizeToTray();
else
    RestoreFromTray();   // single left-click restores
    }
}
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            // If user clicks normal minimize button, also go to tray
            if (WindowState == WindowState.Minimized)
            {
                MinimizeToTray();
            }
        }

        private void MinimizeToTray()
        {
            _trayIcon.Visible = true;
            Hide();
        }

        private void RestoreFromTray()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
            _trayIcon.Visible = false;
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (Visibility == Visibility.Visible)
    MinimizeToTray();
else
    RestoreFromTray();
        }

        private void OnTrayShowClicked(object sender, EventArgs e)
        {
            if (Visibility == Visibility.Visible)
    MinimizeToTray();
else
    RestoreFromTray();
        }

        private void OnTrayExitClicked(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void ToggleSelf(object sender, ExecutedRoutedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                MinimizeToTray();
            }
            else
            {
                if (Visibility == Visibility.Visible)
    MinimizeToTray();
else
    RestoreFromTray();
            }
        }

        // ------------ Overlay controls ------------

        private void EnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateOverlayEnabled();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateOverlayThickness();
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateOverlayOpacity();
        }

        private void WarmButton_Click(object sender, RoutedEventArgs e)
        {
            SetWarmColor();
        }

        private void NeutralButton_Click(object sender, RoutedEventArgs e)
        {
            SetNeutralColor();
        }

        private void CoolButton_Click(object sender, RoutedEventArgs e)
        {
            SetCoolColor();
        }

        private void UpdateOverlayEnabled()
        {
            Overlay.SetEnabled(EnableCheckBox.IsChecked == true);
        }

        private void UpdateOverlayThickness()
        {
            Overlay.UpdateThickness(ThicknessSlider.Value);
        }

        private void UpdateOverlayOpacity()
        {
            Overlay.UpdateOpacity(OpacitySlider.Value);
        }

        private void SetWarmColor()
        {
            Overlay.UpdateColor(Color.FromRgb(255, 244, 230)); // warm
        }

        private void SetNeutralColor()
        {
            Overlay.UpdateColor(Color.FromRgb(245, 245, 245)); // neutral
        }

        private void SetCoolColor()
        {
            Overlay.UpdateColor(Color.FromRgb(220, 235, 255)); // cool
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Application.Current.Shutdown();
        }
    }
}
