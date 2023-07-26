using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using RegistrationClinik.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class MainWindowVIewModel : BaseViewModel
    {
        public MainWindowVIewModel()
        {
            SearchCommand = new LambdaCommand(SearchCommandExecute, CanSearchCommandExecuted);
            ClearCommand = new LambdaCommand(ClearCommandExecute, CanClearCommandExecuted);
            OpenInfoCommand = new LambdaCommand(OpenInfoCommandExecute, CanOpenInfoCommandExecuted);
            GetAllData();
        }

        #region Props

        private ObservableCollection<ShowTableModel> dBTables = new ObservableCollection<ShowTableModel>();
        public ObservableCollection<ShowTableModel> DBTables
        {
            get => dBTables;
            set
            {
                Set(ref dBTables, value);
            }
        }

        private ShowTableModel selectedClient = new();
        public ShowTableModel SelectedClient
        {
            get => selectedClient;
            set
            {
                Set(ref selectedClient, value);
            }
        }
        
        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                Set(ref searchText, value);
            }
        }
        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand OpenInfoCommand { get; set; }

        private bool CanSearchCommandExecuted(object arg) => !string.IsNullOrEmpty(searchText);
        private void SearchCommandExecute(object obj)
        {
            DBTables = new ObservableCollection<ShowTableModel>(DBTables.Where(s => s.Name.Contains(searchText)).OrderBy(s => s.EndTimer));
        }
        private bool CanClearCommandExecuted(object arg) => !string.IsNullOrEmpty(searchText);
        private void ClearCommandExecute(object obj)
        {
            GetAllData();
            searchText = string.Empty;
        }
        private bool CanOpenInfoCommandExecuted(object arg) => true;

        private void OpenInfoCommandExecute(object obj)
        {
            new AllInformationWindow(this,SelectedClient.Id).Show();
        }

        #endregion

        public void GetAllData()
        {
            using ApplicationConnect db = new();
            var query = from d in db.DBTable
                        join kl in db.DBKartrigList on d.Id equals kl.TableId
                        select new
                        {
                            d,
                            kl
                        } 
                        into t1
                        group t1 by t1.kl.TableId into g
                        select new ShowTableModel
                        {
                            Id = g.FirstOrDefault().d.Id,
                            Adres = g.FirstOrDefault().d.Adres,
                            StartTimer = g.FirstOrDefault().kl.StartDate,
                            EndTimer = g.OrderBy(s=>s.kl.EndDate).FirstOrDefault().kl.EndDate,
                            IsActive = g.FirstOrDefault().d.IsActive,
                            Name = g.FirstOrDefault().d.Name,
                            PhoneNumber = g.FirstOrDefault().d.PhoneNumber,
                            ModelName = GetModelName(g.FirstOrDefault().d.ModelId),
                            RowBackground = GetColorOfRow(g.OrderBy(s => s.kl.EndDate).FirstOrDefault().kl.EndDate),
                        }
                        ;
            DBTables = new ObservableCollection<ShowTableModel>(query.OrderBy(s => s.EndTimer));
        }
        public static string GetModelName(int id) 
        {
            using ApplicationConnect db = new();
            return db.DBFilter.FirstOrDefault(s=>s.Id == id)?.Name ?? "Не найдено!";
        }
        public static string GetColorOfRow(DateTime date)
        {
            if ((date.Date - DateTime.Now).Days < 10)
                return "Red";
            else if ((date.Date - DateTime.Now).Days < 30)
                return "DarkOrange";
            else
                return "White";
        }
    }
}
