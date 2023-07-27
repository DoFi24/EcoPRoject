using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class AddTableWindowViewModel : BaseViewModel
    {
        private MainWindowVIewModel viewModel;
        public AddTableWindowViewModel(MainWindowVIewModel _viewModel)
        {
            CreateClientCommand = new LambdaCommand(CreateClientCommandExecute, CanCreateClientCommandExecuted);
            GetAllFilters();
            GetAllKartrijs();
            viewModel = _viewModel;
        }

        #region Props

        private ObservableCollection<DBFilter> filterCollection = new();
        public ObservableCollection<DBFilter> FilterCollection
        {
            get => filterCollection;
            set
            {
                Set(ref filterCollection, value);
            }
        }

        private ObservableCollection<ViewKartrijModel> kartrijCollection = new();
        public ObservableCollection<ViewKartrijModel> KartrijCollection
        {
            get => kartrijCollection;
            set
            {
                Set(ref kartrijCollection, value);
            }
        }

        /*private DBFilter? selectedFilter = new();
        public DBFilter? SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if (value != null && value != new DBFilter())
                {
                    FilterName = value.Name;
                }
                Set(ref selectedFilter, value);

            }
        }*/

        private string? clientName;
        public string? ClientName
        {
            get => clientName;
            set { Set(ref clientName, value); }
        }
        private string? clientPhone;
        public string? ClientPhone
        {
            get => clientPhone;
            set { Set(ref clientPhone, value); }
        }
        private string? clientAdres;
        public string? ClientAdres
        {
            get => clientAdres;
            set { Set(ref clientAdres, value); }
        }
        private DateTime clientStartDate = DateTime.Now;
        public DateTime ClientStartDate
        {
            get => clientStartDate;
            set { Set(ref clientStartDate, value); }
        }

        private DBFilter? clientSelectedFilter = new();
        public DBFilter? ClientSelectedFilter
        {
            get => clientSelectedFilter;
            set { Set(ref clientSelectedFilter, value); }
        }
        #endregion

        #region Commands
        public ICommand CreateClientCommand { get; set; }
        private bool CanCreateClientCommandExecuted(object arg)
        {
            if (clientSelectedFilter is null && clientSelectedFilter == new DBFilter())
                return false;
            if (string.IsNullOrEmpty(clientName) && string.IsNullOrEmpty(clientAdres) && string.IsNullOrEmpty(clientPhone) && string.IsNullOrEmpty(clientSelectedFilter?.Name))
                return false;
            return true;
        }
        private void CreateClientCommandExecute(object obj)
        {
            if (ClientSelectedFilter is null)
                return;
            MessageBox.Show(ClientStartDate.ToShortDateString());
            using ApplicationConnect db = new();
            var result = db.DBTable.Add(new DBTable
            {
                Adres = clientAdres,
                ModelId = clientSelectedFilter.Id,
                Name = clientName,
                PhoneNumber = clientPhone,
                IsActive = 0
            });
            db.SaveChanges();
            foreach (var i in KartrijCollection)
            {
                if (i.IsSelected)
                    db.DBKartrigList.Add(new DBKartrigList 
                    { 
                        SetupDate = ClientStartDate.Date, 
                        StartDate = ClientStartDate.Date,
                        EndDate = ClientStartDate.AddMonths(i.Srok).Date, 
                        KartrijId = i.Id, 
                        TableId = result.Entity.Id 
                    });
            }
            db.SaveChanges();
            viewModel.GetAllData();
            MessageBox.Show("Успешно сохранено!");
        }
        #endregion
        private void GetAllFilters()
        {
            using ApplicationConnect db = new();
            FilterCollection = new ObservableCollection<DBFilter>(db.DBFilter);
        }
        private void GetAllKartrijs()
        {
            using ApplicationConnect db = new();
            var result = db.DBKartrij;
            foreach (var kartrij in result)
            {
                KartrijCollection.Add(new ViewKartrijModel
                {
                    Id = kartrij.Id,
                    Srok = kartrij.Srok,
                    Name = kartrij.Name,
                    IsSelected = false
                });
            }
        }
    }
}
