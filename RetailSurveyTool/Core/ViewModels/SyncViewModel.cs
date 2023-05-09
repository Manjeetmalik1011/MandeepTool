using Jci.RetailSurveyTool.TechnicianApp.Data;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using static Jci.RetailSurveyTool.TechnicianApp.Utility.ErrorHandler;
using System;
using System.Threading.Tasks;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class SyncViewModel : BaseViewModel
    {
        public SyncViewModel(INavigationService navigationService) : base(navigationService)
        {
            SyncCommand = new Command(async () => await ExecuteSyncCommand());
            SyncCommand.Execute(null);
        }

        public override async Task InitializeAsync(object parameter)
        {

        }

        private bool _IsSyncing = true;
        public bool IsSyncing
        {
            set
            {
                //SetProperty(ref _IsSyncing, value);
                _IsSyncing = value;
                IsBusy = value;
            }
            get => _IsSyncing;
        }
        private int _TablesSyncing = 0;

        public int TablesSyncing
        {
            set
            {
                _TablesSyncing = value;
                OnPropertyChanged(nameof(SyncProgress));
                OnPropertyChanged();
            }

            get => _TablesSyncing;
        }

        private int _TablesSynced = 0;
        public int TablesSynced
        {
            get => _TablesSynced;
            set
            {
                _TablesSynced = value;
                OnPropertyChanged(nameof(SyncProgress));
                OnPropertyChanged();
            }
        }

        public double SyncProgress
        {
            get
            {
                return Convert.ToDouble(_TablesSynced) / _TablesSyncing;
            }
        }
        public Command SyncCommand { get; }
        private async Task ExecuteSyncCommand()
        {
            try
            {
                IsSyncing = true;
                await LocalAppDatabase.SyncEntities(false, x => TablesSyncing = x, x => TablesSynced = x);
            }
            catch (Exception e)
            {
                DisplayError("Server Error: Syncing data failed!");
                SendExceptionReport(e);
            }
            finally
            {
                IsSyncing = false;
                //Application.Current.MainPage = new AppShell();
                Application.Current.MainPage = new NavigationPage(new AppShell());
            }

        }
    }
}
