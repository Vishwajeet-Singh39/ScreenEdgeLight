using System;
using System.Windows;
using System.Windows.Media;

namespace ScreenEdgeLight
{
    public partial class MainWindow : Window
    {
        private OverlayWindow Overlay
        {
            get { return App.Overlay; }
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateOverlayEnabled();
            UpdateOverlayThickness();
            UpdateOverlayOpacity();
            SetWarmColor();
        }

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
            Application.Current.Shutdown();
        }
    }
}
