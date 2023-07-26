using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationClinik.ViewModels
{
    public class AllInformationWindowViewModel : BaseViewModel
    {
        public AllInformationWindowViewModel(int _clientId)
        {
            GetAllInfo(_clientId);
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
        //тут берем данные клиента и его картриджы
        private void GetAllInfo(int Id)
        {
            KartrijCollection = new ObservableCollection<KartrijInfoModel>();
            using ApplicationConnect db = new();
            ClientModel = db.DBTable.FirstOrDefault(s => s.Id == Id);
            var result = db.DBKartrigList.Where(s => s.TableId == Id).ToArray();
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
    }
}
