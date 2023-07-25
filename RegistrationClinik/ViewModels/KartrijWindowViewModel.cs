using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class KartrijWindowViewModel : BaseViewModel
    {
        public KartrijWindowViewModel()
        {
            CreateKartrijCommand = new LambdaCommand(CreateKartrijCommandExecute, CanCreateKartrijCommandExecuted);
            EditKartrijCommand = new LambdaCommand(EditKartrijCommandExecute, CanEditKartrijCommandExecuted);
            DeleteKartrijCommand = new LambdaCommand(DeleteKartrijCommandExecute, CanDeleteKartrijCommandExecuted);
            GetAllKartrij();
        }

        #region Props

        private ObservableCollection<DBKartrij> kartrijCollection = new();
        public ObservableCollection<DBKartrij> KartrijCollection
        {
            get => kartrijCollection;
            set
            {
                Set(ref kartrijCollection, value);
            }
        }

        private DBKartrij? selectedKartrij = new();
        public DBKartrij? SelectedKartrij
        {
            get => selectedKartrij;
            set
            {
                if (value != null && value != new DBKartrij())
                {
                    KartrijName = value.Name;
                    KartrijSrok = value.Srok;
                }
                Set(ref selectedKartrij, value);

            }
        }

        private string? kartrijName = string.Empty;
        public string? KartrijName
        {
            get => kartrijName;
            set { Set(ref kartrijName, value); }
        }
        private int kartrijSrok = 0;
        public int KartrijSrok
        {
            get => kartrijSrok;
            set { Set(ref kartrijSrok, value); }
        }

        #endregion

        #region Commands
        public ICommand CreateKartrijCommand { get; set; }
        public ICommand EditKartrijCommand { get; set; }
        public ICommand DeleteKartrijCommand { get; set; }

        private bool CanCreateKartrijCommandExecuted(object arg) => !string.IsNullOrWhiteSpace(KartrijName) && kartrijSrok > 0;
        private void CreateKartrijCommandExecute(object obj)
        {
            if (KartrijName is null && kartrijSrok <= 0) 
                return;
            using ApplicationConnect db = new();
            db.DBKartrij.Add(new DBKartrij { Name = KartrijName, Srok = KartrijSrok});
            db.SaveChanges();
            EndVoid();
        }
        private bool CanEditKartrijCommandExecuted(object arg) => !string.IsNullOrWhiteSpace(KartrijName) && !string.IsNullOrEmpty(SelectedKartrij?.Name) && kartrijSrok > 0;
        private void EditKartrijCommandExecute(object obj)
        {
            if (SelectedKartrij is null)
                return;
            using ApplicationConnect db = new();
            var result = db.DBKartrij.FirstOrDefault(s => s.Id == SelectedKartrij.Id);
            result.Name = KartrijName;
            result.Srok = KartrijSrok;
            db.SaveChanges();
            EndVoid();
        }
        private bool CanDeleteKartrijCommandExecuted(object arg) => !string.IsNullOrEmpty(SelectedKartrij?.Name);
        private void DeleteKartrijCommandExecute(object obj)
        {
            if (SelectedKartrij is null)
                return;
            using ApplicationConnect db = new();
            var result = db.DBKartrij.FirstOrDefault(s => s.Id == SelectedKartrij.Id);
            _ = db.DBKartrij.Remove(result);
            db.SaveChanges();
            EndVoid();
        }

        #endregion
        private void EndVoid()
        {
            KartrijName = string.Empty;
            KartrijSrok = 0;
            MessageBox.Show("Успешно!");
            GetAllKartrij();
        }
        private void GetAllKartrij()
        {
            using ApplicationConnect db = new();
            KartrijCollection = new ObservableCollection<DBKartrij>(db.DBKartrij);
        }
    }
}
