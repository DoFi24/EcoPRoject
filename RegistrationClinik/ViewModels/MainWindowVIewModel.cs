using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using RegistrationClinik.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
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
            NextPageCommand = new LambdaCommand(NextPageCommandExecute, CanNextPageCommandExecuted);
            PreviewPageCommand = new LambdaCommand(PreviewPageCommandExecute, PreviewPageCommandExecuted);
            GetKartrijs();
            GetAllData();
        }

        private void GetKartrijs()
        {
            ApplicationConnect db = new ApplicationConnect();
            AllKartriList = new List<DBKartrigList>(db.DBKartrigList.GroupBy(s => s.TableId)
                    .Select(s => new DBKartrigList
                    {
                        Id = s.Select(s => s.Id).FirstOrDefault(),
                        SetupDate = s.Select(s => s.SetupDate).FirstOrDefault(),
                        StartDate = s.Select(s => s.StartDate).FirstOrDefault(),
                        EndDate = s.Select(s => s.EndDate).FirstOrDefault(),
                        KartrijId = s.Select(s => s.KartrijId).FirstOrDefault(),
                        TableId = s.Select(s => s.TableId).FirstOrDefault(),
                    }).OrderBy(s => s.EndDate));

        }

        #region Props

        List<DBKartrigList> AllKartriList;

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

        private int currentPage = 1;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                Set(ref currentPage, value);
                if (Int32.TryParse(value.ToString(), out currentPage))
                    GetAllData(currentPage);
            }
        }

        private int maxPage = 10;
        public int MaxPage
        {
            get => maxPage;
            set
            {
                Set(ref maxPage, value);
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand OpenInfoCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviewPageCommand { get; set; }
        private bool PreviewPageCommandExecuted(object arg)
        {
            if (currentPage > 1)
                return true;
            return false;
        }

        private bool CanNextPageCommandExecuted(object arg)
        {
            if (currentPage > 0 && currentPage < maxPage)
                return true;
            return false;
        }

        private void PreviewPageCommandExecute(object obj)
        {
            CurrentPage -= 1;
        }

        private void NextPageCommandExecute(object obj)
        {
            CurrentPage += 1;
        }
        private bool CanSearchCommandExecuted(object arg) => !string.IsNullOrEmpty(searchText);
        private void SearchCommandExecute(object obj)
        {
            try
            {
                DBTables = new ObservableCollection<ShowTableModel>(DBTables.Where(s => s.Name.Contains(searchText)).OrderBy(s => s.EndTimer));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
            new AllInformationWindow(this, SelectedClient.Id).Show();
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetAllData(int page = 1)
        {
            try
            {
                DBTables = new ObservableCollection<ShowTableModel>();
                using ApplicationConnect db = new();
                maxPage = (int)Math.Ceiling(db.DBTable.Count() / 100.0);

                var karlist = AllKartriList
                    .Skip((page - 1) * 100)
                    .Take(100);

                var query = (from kl in karlist
                             join d in db.DBTable on kl.TableId equals d.Id
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
                                 SetupDate = g.FirstOrDefault().kl.SetupDate,
                                 StartTimer = g.Min(s => s.kl.StartDate),
                                 EndTimer = g.Max(s => s.kl.EndDate),
                                 IsActive = g.FirstOrDefault().d.IsActive,
                                 Name = g.FirstOrDefault().d.Name,
                                 PhoneNumber = g.FirstOrDefault().d.PhoneNumber,
                                 ModelName = GetModelName(g.FirstOrDefault().d.ModelId),
                                 RowBackground = GetColorOfRow(g.Max(s => s.kl.EndDate))
                             }).ToList();


                //var query = (from d in db.DBTable.Skip((page - 1) * 100).Take(100)
                //             join kl in db.DBKartrigList on d.Id equals kl.TableId
                //             select new
                //             {
                //                 d,
                //                 kl
                //             }
                //             into t1
                //             group t1 by t1.kl.TableId into g
                //             select new ShowTableModel
                //             {
                //                 Id = g.FirstOrDefault().d.Id,
                //                 Adres = g.FirstOrDefault().d.Adres,
                //                 SetupDate = g.FirstOrDefault().kl.SetupDate,
                //                 StartTimer = g.OrderBy(s => s.kl.StartDate).FirstOrDefault().kl.StartDate,
                //                 EndTimer = g.OrderBy(s => s.kl.EndDate).FirstOrDefault().kl.EndDate,
                //                 IsActive = g.FirstOrDefault().d.IsActive,
                //                 Name = g.FirstOrDefault().d.Name,
                //                 PhoneNumber = g.FirstOrDefault().d.PhoneNumber,
                //                 ModelName = GetModelName(g.FirstOrDefault().d.ModelId),
                //                 RowBackground = GetColorOfRow(g.OrderBy(s => s.kl.EndDate).FirstOrDefault().kl.EndDate),
                //             }).OrderBy(s => s.EndTimer);


                foreach (var res in query)
                {
                    DBTables.Add(new ShowTableModel
                    {
                        Id = res.Id,
                        SetupDate = res.SetupDate,
                        StartTimer = res.StartTimer,
                        Adres = res.Adres,
                        EndTimer = res.EndTimer,
                        IsActive = res.IsActive,
                        ModelName = res.ModelName,
                        Name = res.Name,
                        PhoneNumber = res.PhoneNumber,
                        RowBackground = res.RowBackground,
                    });
                }

                //DBTables = new ObservableCollection<ShowTableModel>(DBTables);
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetAll");
                MessageBox.Show(ex.Message);
            }
        }
        public static string GetModelName(int id)
        {
            try
            {
                using ApplicationConnect db = new();
                return db.DBFilter.FirstOrDefault(s => s.Id == id)?.Name ?? "Не найдено!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "Не найдено!";
            }
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
