using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchApp
{
    public partial class WaterMarkedTextBox
    {
        private bool _isFocused;

        public WaterMarkedTextBox()
        {
            InitializeComponent();
            Loaded += WaterMarkedTextBox_Loaded;
        }

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkProperty =
    DependencyProperty.Register(nameof(WaterMark), typeof(string), typeof(WaterMarkedTextBox), new PropertyMetadata(""));

        private void WaterMarkedTextBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(this, 0);
            TextBox innerTextBox = (TextBox)grid.Children[0];
            innerTextBox.GotFocus += WaterMarkedTextBox_GotFocus;
            innerTextBox.LostFocus += WaterMarkedTextBox_LostFocus;
            ChangeWatermarkTextVisibility();
        }

        private void WaterMarkedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _isFocused = false;
            ChangeWatermarkTextVisibility();
        }

        private void WaterMarkedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _isFocused = true;
            ChangeWatermarkTextVisibility();
        }

        private void ChangeWatermarkTextVisibility()
        {
            var grid = (Grid)VisualTreeHelper.GetChild(this, 0);
            TextBlock watermarkText = (TextBlock)grid.Children[1];
            if (!string.IsNullOrEmpty(Text) || _isFocused)
            {
                watermarkText.Visibility = Visibility.Collapsed;
            }
            else
            {
                watermarkText.Visibility = Visibility.Visible;
            }
        }
    }
}
