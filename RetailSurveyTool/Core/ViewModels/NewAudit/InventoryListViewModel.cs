using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class InventoryListViewModel : BaseViewModel
    {
        private List<AlarmTone> dbAlarmTones;
        private List<SystemType> dbSystemTypes;
        private List<DeactivatorType> dbDeactivatorTypes;
        private List<StoreArea> dbStoreArea;

        /// <summary>
        /// UI control properties for List View
        /// </summary>
        const int RefreshDuration = 2;

        bool _isRefreshing;

        public bool IsRefreshing
        {
            //get { return _isRefreshing; }
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Inventory list for the UI List View
        /// </summary>
        private ObservableCollection<Inventory> _inventories;

        public ObservableCollection<Inventory> Inventories
        {
            get => _inventories;
            set
            {
                _inventories = value;
                OnPropertyChanged();
            }
        }

        public InventoryListViewModel(INavigationService navigationService) : base(navigationService)
        {
            Inventories = new ObservableCollection<Inventory>();
            dbAlarmTones = new List<AlarmTone>();
            dbSystemTypes = new List<SystemType>();
            dbDeactivatorTypes = new List<DeactivatorType>();
            dbStoreArea = new List<StoreArea>();
        }

        public override async Task InitializeAsync(object parameter)
        {
        }

        public void InitializeMessenger()
        {
            // set required inputs from other screens
            MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, (newAuditViewModel, audit) => OnStartAudit(audit));
            MessagingCenter.Subscribe<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (newAuditViewModel, customer) => OnSelectedCustomer(customer));
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, StoreArea>(this, MessageNames.SelectedStoreAreaMessage, (auditStoreAreaListViewModel, storeArea) => OnSelectedStoreArea(storeArea));

            // clear inventory list
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel>(this, MessageNames.NewInventoriesListMessage, (auditStoreAreaListViewModel) => OnResetList());
            //MessagingCenter.Subscribe<StoreAreaDetailsViewModel>(this, MessageNames.NewInventoriesListMessage, (storeAreaDetailsViewModel) => OnResetList()); //(this will never hit, delete this line)


            // Refresh inventory list
            MessagingCenter.Subscribe<DeactivationInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage, (deactivationInventoryDetailsViewModel) => OnLoadCommand()); //upon exiting the detail screen
            MessagingCenter.Subscribe<PedestalInventoryDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage, (pedestalInventoryDetailsViewModel) => OnLoadCommand()); //upon exiting the detail screen
            MessagingCenter.Subscribe<StoreAreaDetailsViewModel>(this, MessageNames.RefreshInventoryListMessage, (storeAreaDetailsViewModel) => OnLoadCommand()); //tabs

        }



        /// <summary>
        /// Selected Customer is set by the MessagingCenter
        /// </summary>
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


        /// <summary>
        /// Selected Audit is set by the MessagingCenter
        /// </summary>
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

        /// <summary>
        /// Selected StoreArea is set by the MessagingCenter
        /// </summary>
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


        /// <summary>
        /// Loads the data for the UI List View
        /// </summary>
        public ICommand LoadCommand => new Command(OnLoadCommand);

        private void OnLoadCommand()
        {
            try
            {
                // Load lookup data
                dbAlarmTones.Clear();
                var alarmTones = LocalAppDatabase.GetRawConnection().Table<AlarmTone>().ToListAsync().Result;
                alarmTones.ForEach(x => dbAlarmTones.Add(x));

                dbSystemTypes.Clear();
                var systemTypes = LocalAppDatabase.GetRawConnection().Table<SystemType>().ToListAsync().Result;
                systemTypes.ForEach(x => dbSystemTypes.Add(x));

                dbDeactivatorTypes.Clear();
                var deactivatorTypes = LocalAppDatabase.GetRawConnection().Table<DeactivatorType>().ToListAsync().Result;
                deactivatorTypes.ForEach(x => dbDeactivatorTypes.Add(x));

                dbStoreArea.Clear();
                var storeAreas = LocalAppDatabase.GetRawConnection().Table<StoreArea>().ToListAsync().Result;
                storeAreas.ForEach(x => dbStoreArea.Add(x));

                Debug.WriteLine("Hello! InventoryListViewModel");


                Debug.WriteLine("Hello! InventoryListViewModel - DeactivationInventory");

                // requires StartAudit.ID and SelectedStoreArea.ID
                var dbInventories = LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().Where
                    (x => (x.AuditID == StartAudit.ID && x.StoreAreaID == SelectedStoreArea.ID)).ToListAsync().Result;

                // creates a new list in memory
                var innerjoinresult_as_inventories_list = (from inventory in dbInventories
                                                           join deactivatorType in dbDeactivatorTypes on inventory.DeactivatorTypeID equals deactivatorType.ID
                                                           join storeArea in dbStoreArea on inventory.StoreAreaID equals storeArea.ID

                                                           select new DeactivationInventory
                                                           {
                                                               ID = inventory.ID,

                                                               AuditID = inventory.AuditID,
                                                               Audit = StartAudit,

                                                               StoreAreaID = inventory.StoreAreaID,
                                                               StoreArea = storeArea,

                                                               DeactivatorTypeID = inventory.DeactivatorTypeID,
                                                               DeactivatorType = deactivatorType,

                                                               Qty = inventory.Qty,

                                                               SlimPadCoversNeeded = inventory.SlimPadCoversNeeded,
                                                               NumberOfRegisters = inventory.NumberOfRegisters,
                                                               SelfCheckoutVendor = inventory.SelfCheckoutVendor,
                                                               IsOperational = inventory.IsOperational
                                                           }
                                                        ).ToList();
                Inventories.Clear();

                innerjoinresult_as_inventories_list.ForEach(x => Inventories.Add(x));

                Debug.WriteLine("Hello! InventoryListViewModel - PedestalInventory");

                var dbInventories2 = LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().Where
                    (x => (x.AuditID == StartAudit.ID && x.StoreAreaID == SelectedStoreArea.ID)).ToListAsync().Result;

                //// creates a new list in memory

                var innerjoinresult_as_inventories_list2 = (from inventory in dbInventories2
                                                            join alarmTone in dbAlarmTones on inventory.AlarmToneID equals alarmTone.ID
                                                            join systemType in dbSystemTypes on inventory.SystemTypeID equals systemType.ID
                                                            join storeArea in dbStoreArea on inventory.StoreAreaID equals storeArea.ID

                                                            select new PedestalInventory
                                                            {
                                                                ID = inventory.ID,

                                                                AuditID = inventory.AuditID,
                                                                Audit = StartAudit,

                                                                StoreAreaID = inventory.StoreAreaID,
                                                                StoreArea = storeArea,

                                                                AlarmToneID = inventory.AlarmToneID,
                                                                AlarmTone = alarmTone,

                                                                SystemTypeID = inventory.SystemTypeID,
                                                                SystemType = systemType,

                                                                SystemQty = inventory.SystemQty,
                                                                PedestalQty = inventory.PedestalQty,

                                                                BollardsInstalled = inventory.BollardsInstalled,
                                                                IsOperational = inventory.IsOperational
                                                            }
                                                        ).ToList();
                //Inventories.Clear();

                innerjoinresult_as_inventories_list2.ForEach(x => Inventories.Add(x));



                IsRefreshing = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }


        /// <summary>
        ///  OnResetList method exisit because it's a SingleInstance class defined
        ///  on AppContainer class and these classes havea binding to the UI,
        ///  when loading the form it will call get property for each variable.
        /// </summary>

        private void OnResetList()
        {
            try
            {
                IsRefreshing = false;
                Inventories = new ObservableCollection<Inventory>();
                OnLoadCommand();
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// Selected Inventory item from the UI List View
        /// </summary>

        public ICommand SelectedCommand => new Command<Inventory>(OnSelectedCommand);

        private void OnSelectedCommand(Inventory inventory)
        {


            if (inventory.GetType() == typeof(DeactivationInventory))
            {
                MessagingCenter.Send<InventoryListViewModel>(this, MessageNames.SelectedDeactivationInventoryMessage); // reset inventory detail form
                App.NavigationService.NavigateTo("DeactivationInventoryDetailsPage", inventory);

            }
            else if (inventory.GetType() == typeof(PedestalInventory))
            {
                MessagingCenter.Send<InventoryListViewModel>(this, MessageNames.SelectedPedestalInventoryMessage); // reset inventory detail form
                App.NavigationService.NavigateTo("PedestalInventoryDetailsPage", inventory);
            }
        }

    }
}
