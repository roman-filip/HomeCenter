using System.Windows;
using HomeCenter.Services;

namespace HomeCenter.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly KeyboardListenerService _keyboardListenerService = new();

        public MainWindow()
            => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
            => _keyboardListenerService.Start();

        private void Button_Click_1(object sender, RoutedEventArgs e)
            => _keyboardListenerService.Stop();
    }
}
