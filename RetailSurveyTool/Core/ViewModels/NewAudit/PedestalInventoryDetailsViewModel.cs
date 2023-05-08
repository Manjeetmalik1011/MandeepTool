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
    public partial class PedestalInventoryDetailsViewModel : BaseViewModel
    {
        public PedestalInventoryDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override async Task InitializeAsync(object parameter)
        {
            OnLoadSystemTypes();
            OnLoadAlarmTones();

            // New Pedestal Inventory
            if (parameter == null)
            {


                SelectedInventory = new PedestalInventory()
                {
                    // set default for a new inventory
                    AuditID = StartAudit.ID,
                    StoreAreaID = SelectedStoreArea.ID,
                    Audit = StartAudit,
                    StoreArea = SelectedStoreArea,
                    ID = ((await LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().Where(x => x.ID < 0).OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
                    PedestalQty = 0,
                    SystemQty = 0,
                    BollardsInstalled = false,
                    IsOperational = false

                };

            }
            // Selected Inventory
            else
            {
                try
                {
                    if (SystemTypes != null && SystemTypes.Count > 0 && AlarmTones != null && AlarmTones.Count > 0)
                    {
                        SelectedInventory = parameter as PedestalInventory; // Selected inventory from inventory List                       
                    }
                }
                catch (Exception ex)
                {

                }
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
            MessagingCenter.Subscribe<InventoryListViewModel>(this, MessageNames.SelectedPedestalInventoryMessage, (inventoryListViewModel) => OnResetForm());
        }

        private void OnResetForm()
        {
            SelectedInventory = null;
        }

        public PedestalInventory _selectedInventory;

        public PedestalInventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged();

                if (value != null)
                {

                    _selectedInventory.SystemType = value.SystemType;
                    if (value.SystemType.Name != null)
                    {
                        //  Set picker index for SystemTypes lookup
                        var indexSystemTypes = SystemTypes.IndexOf(SystemTypes.Where(X => X.ID == SelectedInventory.SystemType.ID).FirstOrDefault());
                        if (indexSystemTypes > -1)
                        {
                            _selectedInventory.SystemType = SystemTypes[indexSystemTypes];
                            OnPropertyChanged();
                        }
                    }

                    _selectedInventory.AlarmTone = value.AlarmTone;
                    if (value.AlarmTone.Name != null)
                    {
                        //  Set picker index for AlarmTones lookup
                        var indexAlarmTones = AlarmTones.IndexOf(AlarmTones.Where(X => X.ID == SelectedInventory.AlarmTone.ID).FirstOrDefault());
                        if (indexAlarmTones > -1)
                        {
                            _selectedInventory.AlarmTone = AlarmTones[indexAlarmTones];
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


        public ObservableCollection<AlarmTone> AlarmTones { get; } = new ObservableCollection<AlarmTone>();

        public ICommand LoadAlarmTonesCommand => new Command(OnLoadAlarmTones);

        private void OnLoadAlarmTones()
        {
            try
            {
                if (AlarmTones != null)
                {
                    AlarmTones.Clear();

                    foreach (var item in LocalAppDatabase.GetRawConnection().Table<AlarmTone>().ToListAsync().Result)
                    {
                        AlarmTones.Add(item);
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

        public ObservableCollection<SystemType> SystemTypes { get; } = new ObservableCollection<SystemType>();

        public ICommand LoadSystemTypesCommand => new Command(OnLoadSystemTypes);

        private void OnLoadSystemTypes()
        {
            try
            {
                if (SystemTypes != null)
                {
                    SystemTypes.Clear();

                    foreach (var item in LocalAppDatabase.GetRawConnection().Table<SystemType>().ToListAsync().Result)
                    {
                        SystemTypes.Add(item);
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
            try
            {
                SelectedInventory.AlarmToneID = SelectedInventory.AlarmTone.ID;
                SelectedInventory.SystemTypeID = SelectedInventory.SystemType.ID;
                await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedInventory);
                MessagingCenter.Send<PedestalInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage); //refresh list using DB
                OnResetForm();
                App.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }



        public ICommand DeleteCurrentCommand => new Command(OnDeleteCurrent);

        private async void OnDeleteCurrent()
        {
            await LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().DeleteAsync(x => x.ID == SelectedInventory.ID);
            MessagingCenter.Send<PedestalInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage); //refresh list using DB
            OnResetForm();
            App.NavigationService.GoBack();
        }


    }
}
