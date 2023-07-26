using RegistrationClinik.ViewModels;
using System.Windows;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для regClient.xaml
    /// </summary>
    public partial class regClient : Window
    {
        public regClient(MainWindowVIewModel _view)
        {
            InitializeComponent();
            DataContext = new AddTableWindowViewModel(_view);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
