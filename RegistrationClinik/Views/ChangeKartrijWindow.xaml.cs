using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using RegistrationClinik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RegistrationClinik.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeKartrijWindow.xaml
    /// </summary>
    public partial class ChangeKartrijWindow : Window
    {
        private readonly int changeKartrijId;
        private AllInformationWindowViewModel allVM;
        private int id;

        public ChangeKartrijWindow(AllInformationWindowViewModel _allVM, int id)
        {
            InitializeComponent();
            this.allVM = _allVM;
            changeKartrijId = id;
            //GetAllKartrijData();
        }

        //private void GetAllKartrijData()
        //{
        //    using ApplicationConnect db = new();
        //    var result = db.DBKartrij;
        //    ComboBox1.ItemsSource = new List<DBKartrij>(result);
        //}

        private void Change(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate is null)
                return;

            using ApplicationConnect db = new();
            var result = db.DBKartrigList.FirstOrDefault(s => s.Id == changeKartrijId);
            var item = db.DBKartrij.FirstOrDefault(s => s.Id == result.KartrijId);
            result.StartDate = (DateTime)datePicker.SelectedDate;
            result.EndDate = (DateTime)datePicker.SelectedDate.Value.AddMonths(item.Srok);
            db.SaveChanges();
            MessageBox.Show("Обновлено!");
            allVM.GetAllInfo();
            Close();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
