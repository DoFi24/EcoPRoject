using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System.Collections.ObjectModel;
using System.Data.Common;
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

        public ObservableCollection<DBTable>? dBTables = new ObservableCollection<DBTable>();
        public ObservableCollection<DBTable>? DBTables
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
                db.DBTable.Add(new DBTable());

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

        }
    }
}
