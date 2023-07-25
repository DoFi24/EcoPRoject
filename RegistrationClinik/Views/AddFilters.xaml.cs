using RegistrationClinik.ViewModels;
using System.Windows;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для AddFilters.xaml
    /// </summary>
    public partial class AddFilters : Window
    {
        public AddFilters()
        {
            InitializeComponent();
            DataContext = new FilterWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
