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
        public AllInformationWindowViewModel(int _clientId,AllInformationWindow _window)
        {
            ChangeKartrijCommand = new LambdaCommand(ChangeKartrijCommandExcecute, CanChangeKartrijCommandExcecuted);
            ChangeClientCommand = new LambdaCommand(ChangeClientCommandExcecute, CanChangeClientCommandExcecuted);
            clientId = _clientId;
            window = _window;
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
        private bool CanChangeKartrijCommandExcecuted(object arg)
        {
            return true;
        }
        private void ChangeKartrijCommandExcecute(object obj)
        {
            new ChangeKartrijWindow(this, SelectedKartrij.Id).ShowDialog();
        }
        private bool CanChangeClientCommandExcecuted(object arg)
        {
            return true;
        }
        private void ChangeClientCommandExcecute(object obj)
        {
            using ApplicationConnect db = new();
            var result = db.DBTable.FirstOrDefault(s => s.Id == clientId);
            result.Name = ClientModel.Name;
            result.Adres = ClientModel.Adres;
            result.PhoneNumber = ClientModel.PhoneNumber;
            result.ModelId = ClientSelectedFilter.Id;
            db.SaveChanges();
            MessageBox.Show("Успешно сохранено!");
            window.Close();
        }

        //тут берем данные клиента и его картриджы
        public void GetAllInfo()
        {
            KartrijCollection = new ObservableCollection<KartrijInfoModel>();
            using ApplicationConnect db = new();
            ClientModel = db.DBTable.FirstOrDefault(s => s.Id == clientId);
            ClientSelectedFilter = FilterCollection.FirstOrDefault(s=>s.Id == ClientModel.ModelId);
            var result = db.DBKartrigList.Where(s => s.TableId == clientId).ToArray();
            for (int i = 0; i < result.Length; i++)
                KartrijCollection.Add(new KartrijInfoModel
                {
                    Id = result[i].Id,
                    Number = i+1,
                    StartDate = result[i].StartDate,
                    EndDate = result[i].EndDate,
                    Name = GetKartrijName(result[i].KartrijId)
                });
        }

        //тут берем название модели фильтра
        private void GetFilterName(int Id)
        {
            using ApplicationConnect db = new();
            FilterName = db.DBFilter.FirstOrDefault(s => s.Id == Id).Name;
        }
        private static string GetKartrijName(int Id)
        {
            using ApplicationConnect db = new();
            return db.DBKartrij.FirstOrDefault(s => s.Id == Id).Name;
        }
        private void GetAllFilters()
        {
            using ApplicationConnect db = new();
            FilterCollection = new ObservableCollection<DBFilter>(db.DBFilter);
        }
    }
}
