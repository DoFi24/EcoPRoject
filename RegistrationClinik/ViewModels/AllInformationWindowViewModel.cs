using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using RegistrationClinik.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class AllInformationWindowViewModel : BaseViewModel
    {
        private readonly int clientId;
        private AllInformationWindow window;
        private readonly MainWindowVIewModel mainModel;
        public AllInformationWindowViewModel(int _clientId, AllInformationWindow _window, MainWindowVIewModel _mainModel)
        {
            ChangeKartrijCommand = new LambdaCommand(ChangeKartrijCommandExcecute, CanChangeKartrijCommandExcecuted);
            ChangeClientCommand = new LambdaCommand(ChangeClientCommandExcecute, CanChangeClientCommandExcecuted);
            ChangeKartrijByDateCommand = new LambdaCommand(ChangeKartrijByDateCommandExcecute, CanChangeKartrijByDateCommandExcecuted);
            DeleteClientCommand = new LambdaCommand(DeleteClientCommandExcecute, CanDeleteClientCommandExcecuted);

            clientId = _clientId;
            window = _window;
            mainModel = _mainModel;
            GetAllFilters();
            GetAllInfo();
        }
        public AllInformationWindowViewModel()
        {

        }


        //тут храниться инфо о картрижах клиента
        private ObservableCollection<KartrijInfoModel> kartrijCollection = new();
        public ObservableCollection<KartrijInfoModel> KartrijCollection
        {
            get => kartrijCollection;
            set
            {
                Set(ref kartrijCollection, value);
            }
        }
        private ObservableCollection<DBFilter> filterCollection = new();
        public ObservableCollection<DBFilter> FilterCollection
        {
            get => filterCollection;
            set
            {
                Set(ref filterCollection, value);
            }
        }
        private DBFilter clientSelectedFilter = new();
        public DBFilter ClientSelectedFilter
        {
            get => clientSelectedFilter;
            set
            {
                Set(ref clientSelectedFilter, value);
            }
        }
        //Тут храниться ифно о клиенте
        private DBTable clientModel = new();
        public DBTable ClientModel
        {
            get => clientModel;
            set
            {
                if (value != null && value != new DBTable())
                    GetFilterName(value.ModelId);
                Set(ref clientModel, value);
            }
        }

        private KartrijInfoModel selectedKartrij = new();
        public KartrijInfoModel SelectedKartrij
        {
            get => selectedKartrij;
            set
            {
                Set(ref selectedKartrij, value);
            }
        }

        //тут храниться название модели фильтра
        private string filterName = string.Empty;
        public string FilterName
        {
            get => filterName;
            set
            {
                Set(ref filterName, value);
            }
        }
        public ICommand ChangeKartrijCommand { get; set; }
        public ICommand ChangeClientCommand { get; set; }
        public ICommand DeleteClientCommand { get; set; }

        public ICommand ChangeKartrijByDateCommand { get; set; }
        private bool CanChangeKartrijCommandExcecuted(object arg)
        {
            return true;
        }
        private void ChangeKartrijCommandExcecute(object obj)
        {
            try
            {
                using ApplicationConnect db = new();
                var resylt = db.DBKartrigList.FirstOrDefault(s => s.Id == SelectedKartrij.Id);
                if (resylt != null)
                {
                    resylt.StartDate = DateTime.Now.Date;
                    resylt.EndDate = DateTime.Now.Date.AddMonths(db.DBKartrij.FirstOrDefault(s => s.Id == SelectedKartrij.KarId).Srok);
                    db.SaveChanges();
                    GetAllInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool CanChangeKartrijByDateCommandExcecuted(object arg)
        {
            return true;
        }


        private void ChangeKartrijByDateCommandExcecute(object obj)
        {
            try
            {
                new ChangeKartrijWindow(this, SelectedKartrij.Id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool CanChangeClientCommandExcecuted(object arg)
        {
            return true;
        }
        private void ChangeClientCommandExcecute(object obj)
        {
            try
            {
                using ApplicationConnect db = new();
                var result = db.DBTable.FirstOrDefault(s => s.Id == clientId);
                result.Name = ClientModel.Name;
                result.Adres = ClientModel.Adres;
                result.PhoneNumber = ClientModel.PhoneNumber;
                result.ModelId = ClientSelectedFilter.Id;
                db.SaveChanges();

                mainModel.GetAllData();
                MessageBox.Show("Успешно сохранено!");
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool CanDeleteClientCommandExcecuted(object arg) => true;

        private void DeleteClientCommandExcecute(object obj)
        {
            try
            {
                using (ApplicationConnect db = new ApplicationConnect())
                {
                    var reslut = db.DBTable.FirstOrDefault(s => s.Id == clientId);
                    if (reslut != null)
                        db.Remove(reslut);
                    db.SaveChanges();

                    mainModel.GetAllData();
                    MessageBox.Show("Клиент удален!");
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //тут берем данные клиента и его картриджы
        public void GetAllInfo()
        {
            try
            {
                KartrijCollection = new ObservableCollection<KartrijInfoModel>();
                using ApplicationConnect db = new();
                ClientModel = db.DBTable.FirstOrDefault(s => s.Id == clientId);
                ClientSelectedFilter = FilterCollection.FirstOrDefault(s => s.Id == ClientModel.ModelId);
                var result = db.DBKartrigList.Where(s => s.TableId == clientId).ToArray();
                for (int i = 0; i < result.Length; i++)
                    KartrijCollection.Add(new KartrijInfoModel
                    {
                        Id = result[i].Id,
                        KarId = result[i].KartrijId,
                        Number = i + 1,
                        StartDate = result[i].StartDate,
                        EndDate = result[i].EndDate,
                        Name = GetKartrijName(result[i].KartrijId),
                        RowBackground = GetRowBackground(result[i].EndDate)
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetRowBackground(DateTime date)
        {
            if ((date.Date - DateTime.Now).Days < 10)
                return "Red";
            else if ((date.Date - DateTime.Now).Days < 30)
                return "DarkOrange";
            else
                return "White";
        }

        //тут берем название модели фильтра
        private void GetFilterName(int Id)
        {
            try
            {
                using ApplicationConnect db = new();
                FilterName = db.DBFilter.FirstOrDefault(s => s.Id == Id).Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static string GetKartrijName(int Id)
        {
            try
            {
                using ApplicationConnect db = new();
                return db.DBKartrij.FirstOrDefault(s => s.Id == Id).Name ?? "Удалено!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "Не найдено!";
            }
        }
        private void GetAllFilters()
        {
            try
            {
                using ApplicationConnect db = new();
                FilterCollection = new ObservableCollection<DBFilter>(db.DBFilter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
