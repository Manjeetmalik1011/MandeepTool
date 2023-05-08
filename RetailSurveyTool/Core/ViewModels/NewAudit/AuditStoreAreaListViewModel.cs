using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class AuditStoreAreaListViewModel : BaseViewModel
    {
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

        private Audit _startAudit;

        public Audit StartAudit
        {
            get { return _startAudit; }
            set
            {
                _startAudit = value;
                OnPropertyChanged();
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
            }
        }

        public ObservableCollection<StoreArea> _storeAreas;
        public ObservableCollection<StoreArea> StoreAreas
        {
            get { return _storeAreas; }
            set
            {
                _storeAreas = value;
                OnPropertyChanged();
            }
        }


        public AuditStoreAreaListViewModel(INavigationService navigationService) : base(navigationService)
        {
            CommandTaskAttribute.InitCommands(this);
        }
        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (newAuditViewModel, customer) => OnSelectedCustomer(customer));
            MessagingCenter.Subscribe<PedestalInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage, (pedestalInventoryDetailsViewModel) => OnLoadCommand());
            MessagingCenter.Subscribe<DeactivationInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage, (deactivationInventoryDetailsViewModel) => OnLoadCommand());
            MessagingCenter.Subscribe<IssueDetailViewModel>(this, MessageNames.RefreshIssueListMessage, (issueDetailViewModel) => OnLoadCommand());
            MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, (newAuditViewModel, audit) => OnStartAudit(audit));
        }

        public ICommand LoadCommand => new Command(OnLoadCommand);
        public ICommand SelectedCommand => new Command(OnSelectedCommand);


        private async void OnLoadCommand()
        {
            IsRefreshing = true;
            await LoadStoreAreas();
            IsRefreshing = false;
        }


        private void OnSelectedCommand()
        {
            MessagingCenter.Send<AuditStoreAreaListViewModel>(this, MessageNames.NewIssuesListMessage);
            MessagingCenter.Send<AuditStoreAreaListViewModel, StoreArea>(this, MessageNames.SelectedStoreAreaMessage, SelectedStoreArea);
            App.NavigationService.NavigateTo("StoreAreaDetailsPage");
            ////App.NavigationService.NavigateTo("StoreAreaDetailsPage", SelectedStoreArea);
        }

        public override async Task InitializeAsync(object parameter)
        {
            ////MR Works OnStartAudit(parameter as Audit);
        }

        private async void OnStartAudit(Audit audit)
        {
            _storeAreas = new ObservableCollection<StoreArea>();
            StartAudit = audit;
            await LoadStoreAreas();
        }
        private void OnSelectedCustomer(Customer customer)
        {
            SelectedCustomer = customer;
        }

        private StoreArea _selectedStoreArea;


        public StoreArea SelectedStoreArea
        {
            get { return _selectedStoreArea; }
            set
            {
                _selectedStoreArea = value;
                OnPropertyChanged();
            }
        }


        private async Task LoadStoreAreas()
        {
            var loadedStores = new ObservableCollection<StoreArea>();

            IsBusy = true;
            StoreAreas.Clear();
            _storeAreas.Clear();

            /*var storetype = await LocalAppDatabase.GetRawConnection()
                .Table<StoreType>()
                .Where(x => x.ID == StartAudit.StoreTypeID)
                .FirstOrDefaultAsync();*/



            var query = "SELECT StoreArea.* FROM StoreTypeAreaMap JOIN StoreArea ON StoreTypeAreaMap.StoreAreaID = StoreArea.ID  WHERE StoreTypeAreaMap.StoreTypeID = ? and StoreTypeAreaMap.CustomerID=?";

            var ExistinStoreAreas = LocalAppDatabase.GetRawConnection().QueryAsync<StoreArea>(query, StartAudit.StoreTypeID, SelectedCustomer.ID).Result.ToList();

            foreach (var row in ExistinStoreAreas)
            {
                //check DeactivationInventory and PedestalInventory
                row.Inventories.AddRange(await LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().Where(x => x.StoreAreaID == row.ID && x.AuditID == StartAudit.ID).ToListAsync());
                row.Inventories.AddRange(await LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().Where(x => x.StoreAreaID == row.ID && x.AuditID == StartAudit.ID).ToListAsync());
                row.Issues = await LocalAppDatabase.GetRawConnection().Table<Issue>().Where(x => x.StoreAreaID == row.ID && x.AuditID == StartAudit.ID).ToListAsync();

                row.Icon = await LocalAppDatabase.GetIconAsync(row.IconID);


                loadedStores.Add(row);
            }

            StoreAreas = loadedStores;

            IsBusy = false;
        }


        [CommandTask(nameof(FinishAuditTask))]
        public Command FinishAuditCommand { get; private set; }

        private async Task FinishAuditTask()

        {
            MessagingCenter.Send<AuditStoreAreaListViewModel, Audit>(this, MessageNames.AuditComplete, StartAudit);

            App.NavigationService.NavigateTo("AuditConfirmationPage");


            /*var answer =await App.Current.MainPage.DisplayAlert("Confirmation !", "Are you sure you want to Complete the Audit?", "Yes", "No");

            if (answer)
            {
                StartAudit.Completed = DateTime.UtcNow;
                StartAudit.Status = "Completed";
                await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(StartAudit);

                if (Xamarin.Essentials.Connectivity.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi) || Xamarin.Essentials.Connectivity.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.Cellular))
                {
                    //Connected to WIFI sync immediately
                    Application.Current.MainPage = new SyncPage();
                }
                else
                {
                    //await Shell.Current.GoToAsync("..");
                    App.NavigationService.GoBack();
                }
            }*/

        }

    }
}
