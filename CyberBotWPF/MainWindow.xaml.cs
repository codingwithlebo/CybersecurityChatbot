using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using CyberBotWPF.ViewModels;

namespace CyberBotWPF.Views
{
    public partial class MainWindow : Window
    {
        private readonly ChatViewModel   _vm;
        private readonly DispatcherTimer _clockTimer;

        public MainWindow()
        {
            InitializeComponent();
            _vm = (ChatViewModel)DataContext;

            _clockTimer = new DispatcherTimer
                { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (_, _) =>
                ClockLabel.Text =
                    DateTime.Now.ToString("HH:mm:ss  dd MMM yyyy");
            _clockTimer.Start();

            _vm.Messages.CollectionChanged += (_, _) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    ChatScrollViewer.ScrollToEnd();
                    InputBox.Focus();
                }, DispatcherPriority.Background);
            };

            Loaded += (_, _) =>
            {
                _vm.Initialise();
                InputBox.Focus();
            };
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.SendMessage();
            InputBox.Focus();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _vm.SendMessage();
                InputBox.Focus();
            }
        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string topic)
            {
                _vm.SendQuickTopic(topic);
                InputBox.Focus();
            }
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public static readonly StringToVisibilityConverter Instance = new();

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
            => value is string s && !string.IsNullOrWhiteSpace(s)
                ? Visibility.Visible
                : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}