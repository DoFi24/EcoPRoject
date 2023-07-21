using RegistrationClinik.ViewModels;
using System.Windows;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для regClient.xaml
    /// </summary>
    public partial class regClient : Window
    {
        public MainWindowVIewModel model;

        public regClient(MainWindowVIewModel _model)
        {
            InitializeComponent();
            model = _model;
            DataContext = model;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
