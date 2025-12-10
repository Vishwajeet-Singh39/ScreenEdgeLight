using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
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

            // Tray icon
            _trayIcon = new WinForms.NotifyIcon();
            _trayIcon.Icon = new System.Drawing.Icon("Resources/app_logo.ico");
            _trayIcon.Text = "Screen Edge Light";
            _trayIcon.Visible = false;

            WinForms.ContextMenuStrip menu = new WinForms.ContextMenuStrip();
            menu.Items.Add("Show", null, OnTrayShowClicked);
            menu.Items.Add("Exit", null, OnTrayExitClicked);
            _trayIcon.ContextMenuStrip = menu;

            _trayIcon.MouseUp += TrayIcon_MouseUp;

            // Optional hotkey: Ctrl+Shift+E to toggle control window
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

            // Default color mode
            ApplyWarmColor();
            SelectColorToggle(WarmToggle);
        }

        // ---------- Tray logic ----------

        private void MinimizeToTray_Click(object sender, RoutedEventArgs e)
        {
            MinimizeToTray();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                MinimizeToTray();
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

        private void TrayIcon_MouseUp(object sender, WinForms.MouseEventArgs e)
        {
            if (e.Button == WinForms.MouseButtons.Left)
            {
                if (Visibility == Visibility.Visible && WindowState == WindowState.Normal)
                    MinimizeToTray();
                else
                    RestoreFromTray();
            }
        }

        private void OnTrayShowClicked(object sender, EventArgs e)
        {
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
            if (Visibility == Visibility.Visible && WindowState == WindowState.Normal)
                MinimizeToTray();
            else
                RestoreFromTray();
        }

        // ---------- Enable toggle ----------

        private void EnableToggle_Click(object sender, RoutedEventArgs e)
        {
            UpdateOverlayEnabled();
            UpdateEnableToggleVisuals();
        }

        private void UpdateOverlayEnabled()
        {
            bool enabled = EnableToggle.IsChecked == true;
            Overlay.SetEnabled(enabled);
        }

        private void UpdateEnableToggleVisuals()
        {
            bool enabled = EnableToggle.IsChecked == true;

            if (enabled)
            {
                EnableToggle.Content = "Edge light ON";
            }
            else
            {
                EnableToggle.Content = "Edge light OFF";
            }
            // Background / foreground are driven mostly by style; the
            // template uses IsChecked to switch colors.
        }

        // ---------- Sliders ----------

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateOverlayThickness();
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateOverlayOpacity();
        }

        private void UpdateOverlayThickness()
        {
            Overlay.UpdateThickness(ThicknessSlider.Value);
        }

        private void UpdateOverlayOpacity()
        {
            Overlay.UpdateOpacity(OpacitySlider.Value);
        }

        // ---------- Color modes ----------

        private void SelectColorToggle(ToggleButton selected)
        {
            WarmToggle.IsChecked = (selected == WarmToggle);
            NeutralToggle.IsChecked = (selected == NeutralToggle);
            CoolToggle.IsChecked = (selected == CoolToggle);
        }

        private void WarmToggle_Click(object sender, RoutedEventArgs e)
        {
            SelectColorToggle(WarmToggle);
            ApplyWarmColor();
        }

        private void NeutralToggle_Click(object sender, RoutedEventArgs e)
        {
            SelectColorToggle(NeutralToggle);
            ApplyNeutralColor();
        }

        private void CoolToggle_Click(object sender, RoutedEventArgs e)
        {
            SelectColorToggle(CoolToggle);
            ApplyCoolColor();
        }

        private void ApplyWarmColor()
        {
            Overlay.UpdateColor(Color.FromRgb(255, 244, 230)); // warm
        }

        private void ApplyNeutralColor()
        {
            Overlay.UpdateColor(Color.FromRgb(245, 245, 245)); // neutral
        }

        private void ApplyCoolColor()
        {
            Overlay.UpdateColor(Color.FromRgb(220, 235, 255)); // cool
        }

        // ---------- Shutdown ----------

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Application.Current.Shutdown();
        }
    }
}
