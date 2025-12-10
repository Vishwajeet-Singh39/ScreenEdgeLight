using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ScreenEdgeLight
{
    public partial class OverlayWindow : Window
    {
        private Color _currentColor = Color.FromRgb(255, 244, 230);
        private double _currentOpacity = 0.7;
        private double _currentThickness = 80.0;
        private bool _enabled = true;

        public OverlayWindow()
        {
            InitializeComponent();

            Loaded += OverlayWindow_Loaded;
            SourceInitialized += OverlayWindow_SourceInitialized;
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Left   = SystemParameters.VirtualScreenLeft;
            Top    = SystemParameters.VirtualScreenTop;
            Width  = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;

            ApplySettings();
        }

        private void OverlayWindow_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            NativeMethods.MakeWindowClickThrough(hwnd);   // will compile once NativeMethods is correct
        }

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
            ApplySettings();
        }

        public void UpdateColor(Color color)
        {
            _currentColor = color;
            ApplySettings();
        }

        public void UpdateOpacity(double opacity)
        {
            _currentOpacity = Clamp(opacity, 0.0, 1.0);
            ApplySettings();
        }

        public void UpdateThickness(double thickness)
        {
            if (thickness < 0.0) thickness = 0.0;
            _currentThickness = thickness;
            ApplySettings();
        }

        private double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void ApplySettings()
        {
            if (!_enabled || _currentThickness <= 0.0 || _currentOpacity <= 0.0)
            {
                RootGrid.Visibility = Visibility.Collapsed;
                return;
            }

            RootGrid.Visibility = Visibility.Visible;

            SolidColorBrush brush = new SolidColorBrush(_currentColor);
            brush.Opacity = _currentOpacity;

            TopRect.Fill    = brush;
            BottomRect.Fill = brush;
            LeftRect.Fill   = brush;
            RightRect.Fill  = brush;

            TopRow.Height    = new GridLength(_currentThickness);
            BottomRow.Height = new GridLength(_currentThickness);
            LeftCol.Width    = new GridLength(_currentThickness);
            RightCol.Width   = new GridLength(_currentThickness);
        }
    }
}
