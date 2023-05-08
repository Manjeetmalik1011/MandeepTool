using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class DeactivationInventoryDetailsViewModel : BaseViewModel
    {
        public DeactivationInventoryDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override async Task InitializeAsync(object parameter)
        {
            OnLoadDeactivatorTypesCommand();

            // New Deactivation Inventory
            if (parameter == null)
            {


                SelectedInventory = new DeactivationInventory()
                {
                    // set default for a new inventory
                    AuditID = StartAudit.ID,
                    StoreAreaID = SelectedStoreArea.ID,
                    Audit = StartAudit,
                    StoreArea = SelectedStoreArea,
                    ID = ((await LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().Where(x => x.ID < 0).OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
                    Qty = 0,
                    NumberOfRegisters = 0,
                    SlimPadCoversNeeded = 0,
                    IsOperational = false
                };

            }
            // Selected Inventory
            else
            {
                SelectedInventory = parameter as DeactivationInventory; // Selected inventory from inventory List


            }
        }

        public void InitializeMessenger()
        {
            // required for InitializeAsync new Issue
            MessagingCenter.Subscribe<StoreAreaDetailsViewModel>(this, MessageNames.NewIssueMessage, (storeAreaDetailsViewModel) => OnResetForm());
            MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, (newAuditViewModel, audit) => OnStartAudit(audit));

            // required for LoadIssueTypesCommand
            MessagingCenter.Subscribe<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (newAuditViewModel, customer) => OnSelectedCustomer(customer));
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, StoreArea>(this, MessageNames.SelectedStoreAreaMessage, (auditStoreAreaListViewModel, storeArea) => OnSelectedStoreArea(storeArea));

            // required for selected issue from list view.
            MessagingCenter.Subscribe<InventoryListViewModel>(this, MessageNames.SelectedDeactivationInventoryMessage, (inventoryListViewModel) => OnResetForm());
        }

        private void OnResetForm()
        {
            SelectedInventory = null;
        }

        public DeactivationInventory _selectedInventory;

        public DeactivationInventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged();

                if (value != null)
                {
                    _selectedInventory.DeactivatorType = value.DeactivatorType;
                    if (value.DeactivatorType.Name != null)
                    {
                        //  Set picker index for DeactivatorTypes lookup
                        var indexDeactivatorTypes = DeactivatorTypes.IndexOf(DeactivatorTypes.Where(X => X.ID == SelectedInventory.DeactivatorType.ID).FirstOrDefault());
                        if (indexDeactivatorTypes > -1)
                        {
                            //SelectedDeactivatorType = DeactivatorTypes[indexDeactivatorTypes];
                            _selectedInventory.DeactivatorType = DeactivatorTypes[indexDeactivatorTypes];
                            OnPropertyChanged();
                        }
                    }
                }
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
        private void OnStartAudit(Audit audit)
        {
            StartAudit = audit;
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
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

        private void OnSelectedStoreArea(StoreArea storeArea)
        {
            SelectedStoreArea = storeArea;
        }


        public ObservableCollection<DeactivatorType> DeactivatorTypes { get; } = new ObservableCollection<DeactivatorType>();

        public ICommand LoadDeactivatorTypesCommand => new Command(OnLoadDeactivatorTypesCommand);

        private void OnLoadDeactivatorTypesCommand()
        {
            try
            {
                var query = "SELECT DeactivatorType.* FROM StoreAreaDeactivatorTypeMap JOIN DeactivatorType ON StoreAreaDeactivatorTypeMap.DeactivatorTypeID = DeactivatorType.ID  WHERE StoreAreaDeactivatorTypeMap.StoreAreaID = ?";

                var deactivatorTypes = LocalAppDatabase.GetRawConnection().QueryAsync<DeactivatorType>(query, SelectedStoreArea.ID).Result.ToList();

                if (DeactivatorTypes != null)
                {
                    DeactivatorTypes.Clear();

                    foreach (var item in deactivatorTypes)
                    {
                        DeactivatorTypes.Add(item);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {

            }
        }


        public ICommand SaveCommand => new Command(OnSave);

        private async void OnSave()
        {
            //            SelectedInventory.DeactivatorTypeID = SelectedDeactivatorType.ID;
            SelectedInventory.DeactivatorTypeID = SelectedInventory.DeactivatorType.ID;
            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedInventory);
            MessagingCenter.Send<DeactivationInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage); //refresh list using DB
            OnResetForm();
            App.NavigationService.GoBack();
        }



        public ICommand DeleteCurrentCommand => new Command(OnDeleteCurrent);

        private async void OnDeleteCurrent()
        {
            await LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().DeleteAsync(x => x.ID == SelectedInventory.ID);
            MessagingCenter.Send<DeactivationInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage); //refresh list using DB
            OnResetForm();
            App.NavigationService.GoBack();
        }

    }
}
