using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RegistrationClinik.ViewModels;
using RegistrationClinik.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Key = System.Windows.Input.Key;

namespace RegistrationClinik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVIewModel model;

        public MainWindow()
        {
            InitializeComponent();
            model = new MainWindowVIewModel();
            DataContext = model;
        } 

        private void showAddPage(object sender, RoutedEventArgs e)
        {
            new regClient(model).Show();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Minimize(object sender, RoutedEventArgs e)
        {

        }

        private void showAddKartij(object sender, RoutedEventArgs e)
        {
            new AddKartij().Show();
        }

        private void showAddFilter(object sender, RoutedEventArgs e)
        {
            new AddFilters().Show();
        }
    }
}
