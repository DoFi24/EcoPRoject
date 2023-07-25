using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class MainWindowVIewModel : BaseViewModel
    {
        public MainWindowVIewModel()
        {
            Create = new LambdaCommand(CreateMethod, CanCloseApplicationExecate);
            GetAllData();
        }

        #region Props

        private ObservableCollection<DBTable> dBTables = new ObservableCollection<DBTable>();
        public ObservableCollection<DBTable> DBTables
        {
            get
            {
                return dBTables;
            }
            set
            {
                Set(ref dBTables, value);
            }
        }

        private DBTable? item = new DBTable();
        public DBTable? Item
        {
            get { return item; }
            set { Set(ref item, value); }
        }

        private string? buttonName = "Сохранить";
        public string? ButtonName
        {
            get { return buttonName; }
            set
            {
                Set(ref buttonName, value);
            }
        }
        #endregion

        public ICommand Create { get; set; }
        public void CreateMethod(object o)
        {
            using (ApplicationConnect db = new ApplicationConnect())
            {
                db.SaveChanges();
                GetAllData();
                MessageBox.Show("Данные успешно сохранены");
            }
        }

        public bool CanCloseApplicationExecate(object o)
        {
            return true;
        }
        public void GetAllData()
        {
            dBTables = new ObservableCollection<DBTable>
            {
                new DBTable
                {
                    Id = 1,
                    StartTimer = DateTime.Now,
                    EndTimer = DateTime.Now.AddDays(240),
                    Adres = "Ош",
                    IsActive = 1,
                    Model = new DBFilter{Id = 1,Name = "1"},
                    Name = "Арслеан",
                    PhoneNumber = "88128218"

                },
                 new DBTable
                {
                    Id = 2,
                    StartTimer = DateTime.Now,
                    EndTimer = DateTime.Now.AddDays(240),
                    Adres = "Ош",
                    IsActive = 1,
                    Model = new DBFilter{Id = 1,Name = "1"},
                    Name = "Арслеан",
                    PhoneNumber = "88128218"

                },
                  new DBTable
                {
                    Id = 3,
                    StartTimer = DateTime.Now,
                    EndTimer = DateTime.Now.AddDays(240),
                    Adres = "Ош",
                    IsActive = 1,
                    Model = new DBFilter{Id = 1,Name = "1"},
                    Name = "Арслеан",
                    PhoneNumber = "88128218"

                },
            };
        }
    }
}
