using System.Windows;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для AddKartij.xaml
    /// </summary>
    public partial class AddKartij : Window
    {
        public AddKartij()
        {
            InitializeComponent();
            DataContext = new ViewModels.KartrijWindowViewModel();
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
