using RegistrationClinik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для AllInformationWindow.xaml
    /// </summary>
    public partial class AllInformationWindow : Window
    {
        private MainWindowVIewModel main;
        public AllInformationWindow(MainWindowVIewModel _main,int id)
        {
            InitializeComponent();
            main = _main;
            DataContext = new AllInformationWindowViewModel(id);
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            main.GetAllData();
        }
    }
}
