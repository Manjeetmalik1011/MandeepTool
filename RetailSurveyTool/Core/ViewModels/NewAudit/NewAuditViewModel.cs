using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using static Jci.RetailSurveyTool.TechnicianApp.Utility.ErrorHandler;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public class NewAuditViewModel : BaseViewModel
    {

        //Picker picker = new Picker(); 
        //picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;

        private Audit _selectedAudit;

        //      public NewAuditViewModel(IConnectionService connectionService,INavigationService navigationService, object navigationService1, IDialogService dialogService): base(connectionService,navigationService, dialogService)
        //      {
        //         //MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.PickerSelectedIndexChanged, OnPickerSelectedIndexChanged);
        //      }
        public NewAuditViewModel(INavigationService navigationService) : base(navigationService)
        {
            CommandTaskAttribute.InitCommands(this);
            //            MessagingCenter.Subscribe<CustomerSelectionViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (customerSelectionViewModel, customer) => OnCustomerChanged(customer));
        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<CustomerSelectionViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (customerSelectionViewModel, customer) => OnCustomerChanged(customer));
        }

        // New Audit Page
        public ICommand ServiceCallLookUpCommand => new Command(OnServiceCallLookUpCommand);
        public ICommand SelectCustomerCommand => new Command(OnSelectCustomerCommand);
        public ICommand LoadStoreTypesCommand => new Command(OnLoadStoreTypesCommand);
        public ICommand StartAuditCommand => new Command(OnStartAuditCommand);
        public ICommand SaveCommand => new Command(OnSaveCommand);
        public ICommand PickerSelectedIndexChangedCommand => new Command(OnPickerSelectedIndexChanged);
        //public ICommand PickerSelectedIndexChangedCommand => new Command<StoreType>(OnPickerSelectedIndexChanged);



        public override async Task InitializeAsync(object parameter)
        {
            // Aduit
            SelectedAudit = new Audit(); // to prevent null object, create CreateNewAudit sets the real audit object
            SelectedCustomer = new Customer();
            SelectedStoreType = new StoreType();

            if (parameter == null)
            {
                // New Audit Page
                CreateNewAudit(); //SelectedAudit = (Audit)parameter;
            }
            else
            {
                // set new audit using the job details.
                StartFromWO(parameter as SVMX_WO); //SelectedAudit = (Audit)parameter;
                SelectedAudit = _selectedAudit; // trigger on change event
            }

        }

        public Audit SelectedAudit
        {
            get => _selectedAudit;
            set
            {
                _selectedAudit = value;
                OnPropertyChanged();
            }
        }

        // Audit START
        internal void CreateNewAudit()
        {
            var minID = (LocalAppDatabase.GetRawConnection().Table<Audit>().OrderBy(x => x.ID).FirstOrDefaultAsync().Result?.ID ?? 0) - 1;
            if (minID > 0)
            {
                minID = -1;
            }
            //Audit = new Audit()
            SelectedAudit = new Audit()
            {
                Status = "New",
                AuditTypeID = 1,
                Created = DateTime.UtcNow,
                ID = minID
            };
            WOSearchComplete = false;
        }
        internal void StartFromWO(SVMX_WO sourceWorkOrder)
        {
            IsBusy = true;
            var existingAudit = LocalAppDatabase.GetRawConnection().Table<Audit>().FirstOrDefaultAsync(x => x.ServiceCallNumber == sourceWorkOrder.WORK_ORDER).Result;
            if (existingAudit == null)
            {
                //CreateNewAudit(); //moved to Initialize
                var minID = (LocalAppDatabase.GetRawConnection().Table<Audit>().OrderBy(x => x.ID).FirstOrDefaultAsync().Result?.ID ?? 0) - 1;
                if (minID > 0)
                {
                    minID = -1;
                }
                //Audit = new Audit()
                _selectedAudit.Status = "New";
                _selectedAudit.AuditTypeID = 1;
                _selectedAudit.Created = DateTime.UtcNow;
                _selectedAudit.ID = minID;
            }
            else
            {
                SelectedAudit = existingAudit;
            }
            ApplyWOtoAudit(sourceWorkOrder);
            WOSearchComplete = true; //pass validation to allow starting a audit because a check has been performed 
            IsBusy = false;
        }
        private void ApplyWOtoAudit(SVMX_WO sourceWorkOrder)
        {



            
            _selectedAudit.ServiceCallNumber = sourceWorkOrder.WORK_ORDER;
            _selectedAudit.BobWOId = sourceWorkOrder?.WORK_ORDER_ID;
            _selectedAudit.LocationName = sourceWorkOrder?.LOCATION_NAME;
            _selectedAudit.Address = sourceWorkOrder?.ADDRESS;
            _selectedAudit.City = sourceWorkOrder?.CITY;
            _selectedAudit.State = sourceWorkOrder?.STATE;
            _selectedAudit.Zip = sourceWorkOrder?.ZIP;

            if (int.TryParse(sourceWorkOrder.ERP_CUST_NUM, out int erpCustFromWO))
            {
                var erpMapping = LocalAppDatabase.GetRawConnection().Table<CustomerErpMapping>().FirstOrDefaultAsync(x => x.ErpCustomerNumber == erpCustFromWO).Result;
                if (erpMapping != null)
                {
                    //sets private int? CustomerID => SelectedCustomer?.ID;
                    SelectedCustomer = LocalAppDatabase.GetCustomerAsync(erpMapping.CustomerID).Result;
                }
                else
                {
                    CustomerManualSelectionReason = "Unable To Determine Customer";
                }
            }
            var extractedStoreNum = sourceWorkOrder.GetStoreNumber();
            if (extractedStoreNum != null)
            {
                _selectedAudit.StoreNumber = extractedStoreNum;
            }
        }

        // Audit END


        // CUSTOMER START
        // required to enable store type field
        // this flag is only set when the customer is selected. default it's false.
        private bool _customerIsSelected;
        public bool CustomerIsSelected
        {
            set => SetProperty(ref _customerIsSelected, value);
            get => _customerIsSelected;
        }


        // selected customer from customer selection screen or job list screen
        private int? CustomerID => SelectedCustomer?.ID;

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                _customerIsSelected = true;
                OnPropertyChanged(nameof(CustomerID));
                OnLoadStoreTypesCommand();
                OnPropertyChanged(nameof(StoreTypes));
                OnPropertyChanged();
            }
        }
        // CUSTOMER END



        // New Audit Page
        private async void OnServiceCallLookUpCommand()
        {
            //TODO Fetch SMAX Data
            try
            {
                var wo = await RestService.GetWOByNumberAsync(SelectedAudit.ServiceCallNumber);
                if (wo != null)
                {
                    Debug.WriteLine("CustomerSelectionPage not required");
                    ApplyWOtoAudit(wo);
                }
                else
                {
                    Debug.WriteLine("CustomerSelectionPage required!!!");
                    CustomerManualSelectionReason = "WO Not Found";
                    await ShowAlert(CustomerManualSelectionReason, "Customer Must Manually Be Selected", "Ok");
                    //await Shell.Current.GoToAsync("CustomerSelectionPage");
                    App.NavigationService.NavigateTo("CustomerSelectionPage");
                }
                WOSearchComplete = true; //pass validation to allow starting a audit because a check has been performed 
            }
            catch (Exception e)
            {
                DisplayError("Server error, failed to search the WO!");
                SendExceptionReport(e);
            }

        }

        private void OnSelectCustomerCommand()
        {
            App.NavigationService.NavigateTo("CustomerSelectionPage");

        }

        // StartAudit
        private async void OnStartAuditCommand()
        {
            // validation before starting the audit
            if (WOSearchComplete == false)
            {
                await ShowAlert("Info", "Please enter a WO# and do a search first", "Ok");
                return;
            }
            if (SelectedCustomer == null)
            {
                await ShowAlert("Info", "Please select a customer first", "Ok");
                return;
            }
            if (SelectedStoreType == null)
            {
                await ShowAlert("Info", "Please select a store type", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(SelectedAudit.StoreNumber))
            {
                await ShowAlert("Info", "Please enter a store number", "Ok");
                return;
            }
            //await LoadStoreAreas();

            //MessagingCenter.Send<Audit>(SelectedAudit, MessageNames.StartAuditMessage);
            //MessagingCenter.Send<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, SelectedAudit);

            //MessagingCenter.Send<NewAuditViewModel, string>(this, "alert", "Hello World!");
            //MessagingCenter.Send<NewAuditViewModel>(this, "Hi");


            MessagingCenter.Send<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, SelectedCustomer);

            MessagingCenter.Send<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, SelectedAudit);
            App.NavigationService.NavigateTo("AuditStoreAreaList");

            ////Works App.NavigationService.NavigateTo("AuditStoreAreaList", SelectedAudit);


        }

        private void OnSaveCommand()
        {
            if (String.IsNullOrEmpty(SelectedAudit.ID.ToString()))
                //App.AuditDataServie.AddAudit(SelectedAudit);
                Debug.WriteLine("AddAudit");
            else
                //App.AuditDataServie.UpdateAudit(SelectedAudit);
                Debug.WriteLine("UpdateAudit");


            MessagingCenter.Send<NewAuditViewModel, Audit>(this, MessageNames.AuditChangedMessage, SelectedAudit);
            App.NavigationService.GoBack();
        }

        //private void OnCustomerChanged(CustomerSelectionViewModel sender, Customer customer)
        //{
        //    // set NewAuditViewModel SelectedCustomer from callback CustomerSelectionViewModel
        //    SelectedCustomer = customer;
        //}

        private void OnCustomerChanged(Customer customer)
        {
            SelectedCustomer = customer;
        }


        //StoreType Start

        public ObservableCollection<StoreType> StoreTypes { get; } = new ObservableCollection<StoreType>();
        //public ObservableCollection<StoreType> StoreTypes = new ObservableCollection<StoreType>();

        private void OnLoadStoreTypesCommand()
        {
            // set current selected to blank
            SelectedStoreType = new StoreType();

            IsBusy = true;

            try
            {
                StoreTypes.Clear();
                if (CustomerID == null)
                {
                    _customerIsSelected = false;
                    return;
                }

                //var items = LocalAppDatabase.GetAuditTypeAsync().Result;

                //var customer =  LocalAppDatabase.GetRawConnection().Table<Customer>().Where(x => x.ID == CustomerID).FirstAsync().Result;

                var query = "SELECT StoreType.* FROM CustomerStoreMap JOIN StoreType ON CustomerStoreMap.StoreTypeId = StoreType.ID  WHERE CustomerStoreMap.CustomerId = ?";

                var storetypes = LocalAppDatabase.GetRawConnection().QueryAsync<StoreType>(query, CustomerID).Result.ToList();

                //var customer = LocalAppDatabase.GetRawConnection().GetWithChildrenAsync<Customer>(CustomerID, recursive: true).Result;

                if (storetypes != null)
                {
                    foreach (var item in storetypes)
                    {
                        StoreTypes.Add(item);
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private StoreType _selectedStoreType;

        //public StoreType SelectedStoreType
        //{
        //    get => _selectedStoreType;
        //    set
        //    {
        //        _selectedStoreType = value;
        //        SelectedAudit.StoreTypeID = value.ID; //for db
        //        //OnPropertyChanged(nameof(StoreTypes));
        //        OnPropertyChanged();
        //    }
        //}

        public StoreType SelectedStoreType
        {
            get { return _selectedStoreType; }
            set
            {
                _selectedStoreType = value;
                OnPropertyChanged();
            }
        }


        private void OnPickerSelectedIndexChanged()
        {
            Debug.WriteLine("Hello! NewAuditViewModel");
            //            Debug.WriteLine("SelectedAudit.StoreTypeID: " + SelectedAudit.StoreTypeID);
            //            Debug.WriteLine("SelectedStoreType.ID: " + SelectedStoreType.ID);


            if (SelectedStoreType != null)
            {
                SelectedAudit.StoreTypeID = SelectedStoreType.ID;
            }
            else
            {
                Debug.WriteLine("do nothing NewAuditViewModel");
            }


        }

        //public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;

        //    if (selectedIndex != -1)
        //    {
        //        Debug.WriteLine(selectedIndex.ToString());
        //        //monkeyNameLabel.Text = picker.Items[selectedIndex];
        //    }
        //}
        //StoreType END






        // BAD //
        const string DEFAULT_MANUAL_REASON = "Notice";

        string CustomerManualSelectionReason = DEFAULT_MANUAL_REASON;





        //public Command LoadAuditTypesCommand { get; private set; }
        //async Task ExecuteLoadAuditTypesCommand()
        //{
        //    IsBusy = true;

        //    try
        //    {
        //        AuditTypes.Clear();
        //        var items = await LocalAppDatabase.GetAuditTypeAsync();
        //        foreach (var item in await LocalAppDatabase.GetRawConnection().Table<AuditType>().Where(x => x.CustomerID == null || x.CustomerID == CustomerID).ToListAsync())
        //        {
        //            AuditTypes.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}




        bool allowCustomerSelection = false;

        bool WOSearchComplete = false;
        private static async Task ShowAlert(string Title, string Message, string Cancel)
        {
            await Application.Current.MainPage.DisplayAlert(Title, Message, Cancel);
        }



        //public Command SelectCustomerCommand { get; }
        //public Audit Audit { set; get; }


    }
}
