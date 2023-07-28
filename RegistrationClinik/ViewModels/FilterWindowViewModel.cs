using RegistrationClinik.Infras;
using RegistrationClinik.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RegistrationClinik.ViewModels
{
    public class FilterWindowViewModel : BaseViewModel
    {
        public FilterWindowViewModel()
        {
            CreateFilterCommand = new LambdaCommand(CreateFilterCommandExecute, CanCreateFilterCommandExecuted);
            EditFilterCommand = new LambdaCommand(EditFilterCommandExecute, CanEditFilterCommandExecuted);
            DeleteFilterCommand = new LambdaCommand(DeleteFilterCommandExecute, CanDeleteFilterCommandExecuted);
            GetAllFilters();
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

        private DBFilter? selectedFilter = new();
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
        }

        private string? filterName;
        public string? FilterName
        {
            get => filterName;
            set { Set(ref filterName, value); }
        }

        #endregion

        #region Commands
        public ICommand CreateFilterCommand { get; set; }
        public ICommand EditFilterCommand { get; set; }
        public ICommand DeleteFilterCommand { get; set; }

        private bool CanCreateFilterCommandExecuted(object arg) => !string.IsNullOrWhiteSpace(FilterName);
        private void CreateFilterCommandExecute(object obj)
        {
            using ApplicationConnect db = new();
            db.DBFilter.Add(new DBFilter { Name = FilterName });
            db.SaveChanges();
            EndVoid();
            MessageBox.Show("Добавлено!");

        }
        private bool CanEditFilterCommandExecuted(object arg) => !string.IsNullOrWhiteSpace(FilterName) && !string.IsNullOrEmpty(SelectedFilter?.Name);
        private void EditFilterCommandExecute(object obj)
        {
            if (SelectedFilter is null)
                return;
            using ApplicationConnect db = new();
            var result = db.DBFilter.FirstOrDefault(s => s.Id == SelectedFilter.Id);
            result.Name = FilterName;
            db.SaveChanges();
            EndVoid();
            MessageBox.Show("Изменено!");

        }
        private bool CanDeleteFilterCommandExecuted(object arg) => !string.IsNullOrEmpty(SelectedFilter?.Name);
        private void DeleteFilterCommandExecute(object obj)
        {
            if (SelectedFilter is null)
                return;
            using ApplicationConnect db = new();
            db.DBFilter.FirstOrDefault(s => s.Id == SelectedFilter.Id).IsActive = false;
            db.SaveChanges();
            EndVoid();
            MessageBox.Show("Удалено!");
        }

        #endregion
        private void EndVoid()
        {
            FilterName = string.Empty;
            GetAllFilters();
        }
        private void GetAllFilters()
        {
            using ApplicationConnect db = new();
            FilterCollection = new ObservableCollection<DBFilter>(db.DBFilter.Where(s => s.IsActive));
        }
    }
}
