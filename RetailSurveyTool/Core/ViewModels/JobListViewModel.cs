using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class JobListViewModel : BaseViewModel
    {

        private ObservableCollection<SVMX_WO> _wos;

        public JobListViewModel(INavigationService navigationService) : base(navigationService)
        {
            WOs = new ObservableCollection<SVMX_WO>();
            Task task = SyncAudits();
            MessagingCenter.Subscribe<NewAuditViewModel, Audit>
            (this, MessageNames.AuditChangedMessage, OnWOChanged);
        }
        private SVMX_WO _woselectedItem;
        public SVMX_WO WoSelectedItem
        {
            get => _woselectedItem;
            set
            {
                if (value != null)
                {
                    _woselectedItem = value;
                    OnPropertyChanged("WoSelectedItem");
                    App.NavigationService.NavigateTo("NewAuditPage", value);
                    WoSelectedItem = null;
                }

            }
        }
        public ICommand LoadCommand => new Command(OnLoadCommand);
        public ICommand AddCommand => new Command(OnAddCommand);


        public ObservableCollection<SVMX_WO> WOs
        {
            get => _wos;
            set
            {
                _wos = value;
                OnPropertyChanged();
            }
        }

        private async void OnLoadCommand()
        {
            await SyncAudits();
        }


        private void OnAddCommand()
        {
            App.NavigationService.NavigateTo("NewAuditPage");
        }


        private async Task SyncAudits()
        {
            IsBusy = true;
            IsRefreshing = true;
            var dbAudits = await LocalAppDatabase.GetRawConnection().Table<SVMX_WO>().Where(x => !x.Consumed).ToListAsync();
            WOs.Clear();
            dbAudits.ForEach(x => WOs.Add(x));
            IsBusy = false;
            IsRefreshing = false;
            NoAuditsFound = WOs.Count == 0;
        }

        const int RefreshDuration = 2;

        bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private bool noAuditsFound = false;

        public bool NoAuditsFound { get => noAuditsFound; set => SetProperty(ref noAuditsFound, value); }

        private void OnWOChanged(NewAuditViewModel sender, Audit audit)
        {
            Debug.WriteLine("OnWOChanged - should never be called");
        }

        public override async Task InitializeAsync(object data)
        {

        }

    }
}
