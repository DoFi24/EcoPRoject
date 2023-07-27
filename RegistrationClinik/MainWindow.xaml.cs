using RegistrationClinik.ViewModels;
using RegistrationClinik.Views;
using System.Windows;

namespace RegistrationClinik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVIewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowVIewModel();
            DataContext = viewModel;
        } 
        private void ShowAddPage(object sender, RoutedEventArgs e)
        {
            new regClient(viewModel).Show();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ShowAddKartij(object sender, RoutedEventArgs e)
        {
            new AddKartij().Show();
        }

        private void ShowAddFilter(object sender, RoutedEventArgs e)
        {
            new AddFilters().Show();
        }

        private void Normal(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
