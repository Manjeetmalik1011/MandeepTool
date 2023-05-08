using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class StoreAreaDetailsViewModel : BaseViewModel
    {
        public StoreAreaDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
            CommandTaskAttribute.InitCommands(this);
        }

        public void InitializeMessenger()
        {
            //MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, (newAuditViewModel, audit) => OnStartAudit(audit));
            //MessagingCenter.Subscribe<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (newAuditViewModel, customer) => OnSelectedCustomer(customer));
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, StoreArea>(this, MessageNames.SelectedStoreAreaMessage, (auditStoreAreaListViewModel, storeArea) => OnSelectedStoreArea(storeArea));
        }

        private StoreArea _selectedStoreArea;

        public StoreArea SelectedStoreArea
        {
            get { return _selectedStoreArea; }
            set
            {
                _selectedStoreArea = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StoreAreaDetailTitle));
            }
        }

        public ICommand AddItemCommand => new Command(OnAddItemCommand);
        public ICommand AddInventoryCommand => new Command(OnAddInventoryCommand);
        public ICommand AddIssueCommand => new Command(OnAddIssueCommand);


        private void OnAddItemCommand()
        {
            if ((SelectedTab?.GetType() ?? typeof(InventoryListPage)) == typeof(InventoryListPage))
            {
                //MessagingCenter.Send<StoreAreaDetailsViewModel>(this, MessageNames.NewInventoriesListMessage); //Reset inventoroies list form // not used 
                OnAddInventoryCommand();
            }
            else
            {
                //MessagingCenter.Send<StoreAreaDetailsViewModel>(this, MessageNames.NewIssuesListMessage); //Reset issue list form // not used 
                OnAddIssueCommand();
            }
        }

        private void OnAddInventoryCommand()
        {
            if (SelectedStoreArea.DeactivationArea && SelectedStoreArea.PedestalArea)
            {
                App.NavigationService.NavigateTo("SelectInventoryPage");
            }
            else if (SelectedStoreArea.DeactivationArea)
            {
                App.NavigationService.NavigateTo("DeactivationInventoryDetailsPage");
            }
            else if (SelectedStoreArea.PedestalArea)
            {
                App.NavigationService.NavigateTo("PedestalInventoryDetailsPage");
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Inventory", "Pedestal or Deactivation type is not set for this area", "Ok");
            }
        }

        private void OnAddIssueCommand()
        {
            App.NavigationService.NavigateTo("IssueDetailPage");
        }

        private void OnSelectedStoreArea(StoreArea storeArea)
        {
            SelectedStoreArea = storeArea;
        }


        public override async Task InitializeAsync(object parameter)
        {

        }

        public string StoreAreaDetailTitle
        {
            get
            {
                return SelectedStoreArea?.Name ?? "Area Details";
            }
        }

        private Page _selectedTab;

        public Page SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
                tabChanged();
            }
        }


        private void tabChanged()
        {
            if ((SelectedTab?.GetType() ?? typeof(InventoryListPage)) == typeof(InventoryListPage))
            {
                MessagingCenter.Send<StoreAreaDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage); //Reset issue list form
            }
            else
            {
                MessagingCenter.Send<StoreAreaDetailsViewModel>(this, MessageNames.RefreshIssueListMessage); //Reset issue list form
            }
        }






























        //////[CommandTask(nameof(StoreAreaSelected))]
        //////public Command StoreAreaSelectedCommand { get; private set; }

        //////async Task StoreAreaSelected()
        //////{
        //////    ////Issues.Clear();
        //////    ////Inventories.Clear();
        //////    ////SelectedStoreArea.Inventories.ForEach(x => Inventories.Add(x));
        //////    ////SelectedStoreArea.Issues.ForEach(x => Issues.Add(x));
        //////    //await Shell.Current.GoToAsync(nameof(StoreAreaDetailsPage));
        //////}


        //////public ObservableCollection<StoreArea> StoreAreas { get; } = new ObservableCollection<StoreArea>();


        //////public async Task LoadExistingData(int id)
        //////{
        //////    StartAudit = await LocalAppDatabase.restService.GetAuditAsync(id);
        //////    StoreAreas.Clear();
        //////    foreach (var area in StartAudit.StoreType.StoreAreas)
        //////    {
        //////        StoreAreas.Add(area);
        //////    }
        //////}

        //////public ObservableCollection<AuditType> AuditTypes { get; } = new ObservableCollection<AuditType>();


        //////public Issue SelectedIssue { set; get; }

        //////public IssueCategory SelectedIssueCategory
        //////{
        //////    get => SelectedIssue.IssueCategory; 
        //////    set => SelectedIssue.IssueCategory = value;
        //////}


        //////private int detailPageIndex = 0;


        //////public Command GoBackCommand { get; }



    }
}
