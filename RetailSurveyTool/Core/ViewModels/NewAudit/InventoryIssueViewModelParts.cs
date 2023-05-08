using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public class InventoryIssueViewModelParts : BaseViewModel //partial //StoreAreaDetailsViewModel //AuditStoreAreaListViewModel //NewAuditViewModel
    {
        /// <summary>
        /// Start StoreAreaDetailsViewModel
        /// </summary>
        /// 

        private Page _selectedTab;

        public Page SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
                ////OnPropertyChanged(nameof(AddItemCommand));
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


        private const string PEDESTAL_CONST = "Pedestal";

        public ObservableCollection<StoreArea> StoreAreas { get; } = new ObservableCollection<StoreArea>();

        public ObservableCollection<AlarmTone> AlarmTones { get; } = new ObservableCollection<AlarmTone>();
        public ObservableCollection<SystemType> SystemTypes { get; } = new ObservableCollection<SystemType>();
        public ObservableCollection<DeactivatorType> DeactivatorTypes { get; } = new ObservableCollection<DeactivatorType>();
        /// <summary>
        /// END StoreAreaDetailsViewModel
        /// </summary>

        public ObservableCollection<Inventory> Inventories { get; } = new ObservableCollection<Inventory>();
        public ObservableCollection<Issue> Issues { get; } = new ObservableCollection<Issue>();

        // public ObservableCollection<Picture> Pictures { get; } = new ObservableCollection<Picture>();
        public PedestalInventory SelectedPedInventory { set; get; }
        public DeactivationInventory SelectedDeactInventory { set; get; }
        private void RefreshItemAreaPage()
        {
            foreach (var area in StoreAreas)
            {
                area.NotifyPropertyChanged(nameof(area.IssueStatus));

                area.NotifyPropertyChanged(nameof(area.InventoryStatus));
            }
        }
        private void Issues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var area in StoreAreas)
            {
                area.NotifyPropertyChanged(nameof(area.IssueStatus));
            }
        }

        private void Inventories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var area in StoreAreas)
            {
                area.NotifyPropertyChanged(nameof(area.InventoryStatus));
            }
        }

        async Task AsyncLoad<T>(Func<Task<List<T>>> Source, ObservableCollection<T> Destination)
        {
            var items = await Source.Invoke();
            lock (Destination)
            {
                Destination.Clear();
                foreach (var item in items)
                {
                    Destination.Add(item);
                }
            }
        }


        private async Task AddInventoryTask()
        {
            if (AlarmTones.Count == 0)
            {
                await Task.WhenAll(
                    AsyncLoad(LocalAppDatabase.GetAlarmToneAsync, AlarmTones),
                    AsyncLoad(LocalAppDatabase.GetSystemTypeAsync, SystemTypes),
                    AsyncLoad(LocalAppDatabase.GetDeactivatorTypeAsync, DeactivatorTypes)
                );
            }


            //await Shell.Current.GoToAsync(null);
            bool showPedestal = _selectedStoreArea.PedestalArea;
            if (_selectedStoreArea.DeactivationArea && _selectedStoreArea.PedestalArea)
            {
                string action = await Shell.Current.DisplayActionSheet("Type To Inventory", "Cancel", null, new[] { "Deactivation", PEDESTAL_CONST });
                if (action == "Cancel")
                    return;
                else
                {
                    showPedestal = action == PEDESTAL_CONST;
                }
            }
            if (showPedestal)
            {
                var newInv = new PedestalInventory()
                {
                    AuditID = StartAudit.ID,
                    StoreAreaID = SelectedStoreArea.ID,
                    Audit = StartAudit,
                    StoreArea = SelectedStoreArea,
                    ID = ((await LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
                    PedestalQty = 0,
                    SystemQty = 0,
                    BollardsInstalled = false,
                };
                SelectedPedInventory = newInv;
                StartAudit.Inventories.Add(newInv);
                _selectedStoreArea.Inventories.Add(newInv);
                Inventories.Add(newInv);
                await Shell.Current.GoToAsync(nameof(PedestalInventoryDetailsPage));
            }
            else
            {
                var newInv = new DeactivationInventory()
                {
                    AuditID = StartAudit.ID,
                    StoreAreaID = SelectedStoreArea.ID,
                    Audit = StartAudit,
                    StoreArea = SelectedStoreArea,
                    ID = ((await LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
                    Qty = 0,
                    NumberOfRegisters = 0,
                    SlimPadCoversNeeded = 0,
                };
                StartAudit.Inventories.Add(newInv);
                _selectedStoreArea.Inventories.Add(newInv);
                Inventories.Add(newInv);
                SelectedDeactInventory = newInv;
                await Shell.Current.GoToAsync(nameof(DeactivationInventoryDetailsPage));
            }
        }
        [CommandTask(nameof(AddInventoryTask))]
        public Command AddInventory { get; private set; }
        public Command AddItemCommand
        {
            get
            {
                if ((SelectedTab?.GetType() ?? typeof(InventoryListPage)) == typeof(InventoryListPage))
                {
                    return AddInventory;
                }
                else
                {
                    return AddIssue;
                }
            }
        }

        public Issue SelectedIssue { set; get; }
        [CommandTask(nameof(AddIssueTask))]
        public Command AddIssue { get; private set; }

        private async Task AddIssueTask()
        {
            var newIssue = new Issue()
            {
                AuditID = StartAudit.ID,
                StoreAreaID = SelectedStoreArea.ID,
                Audit = StartAudit,
                StoreArea = SelectedStoreArea,
                ID = ((await LocalAppDatabase.GetRawConnection().Table<Issue>().OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
            };
            StartAudit.Issues.Add(newIssue);
            _selectedStoreArea.Issues.Add(newIssue);
            Issues.Add(newIssue);
            SelectedIssue = newIssue;
            await Shell.Current.GoToAsync(nameof(IssueDetailPage));
        }

        public IssueType SelectedIssueType
        {
            get
            {
                return SelectedIssue.IssueType;
            }
            set
            {
                SelectedIssue.IssueType = value;
                OnPropertyChanged();
                LoadIssueCategoriesCommand?.Execute(null);
            }
        }
        [CommandTask(nameof(LoadIssueTypes))]
        public Command LoadIssueTypesCommand { get; private set; }
        public ObservableCollection<IssueType> IssueTypes { get; } = new ObservableCollection<IssueType>();
        public async Task LoadIssueTypes()
        {
            IssueTypes.Clear();
            try
            {
                var query = "SELECT IssueType.* FROM CustomerIssueTypeMap JOIN IssueType ON CustomerIssueTypeMap.IssueTypeID = IssueType.ID  WHERE CustomerIssueTypeMap.CustomerID = ? AND (IssueType.ShowForDeactivation=? OR  x.ShowForSystem =?)";

                var issueTypes = LocalAppDatabase.GetRawConnection().QueryAsync<IssueType>(query, StartAudit.ID, SelectedStoreArea.DeactivationArea, SelectedStoreArea.PedestalArea).Result.ToList();


                /* var items = await LocalAppDatabase.GetRawConnection().Table<IssueType>().Where(x => (x.ShowForDeactivation == SelectedStoreArea.DeactivationArea || x.ShowForSystem == SelectedStoreArea.PedestalArea) && (x.CustomerID == null || x.CustomerID == StartAudit.ID)).ToListAsync();*/

                foreach (var it in issueTypes)
                {
                    lock (IssueTypes)
                    {
                        IssueTypes.Add(it);
                    }
                }
            }
            catch { }
        }
        [CommandTask(nameof(LoadIssueCategories))]
        public Command LoadIssueCategoriesCommand { get; private set; }
        public ObservableCollection<IssueCategory> IssueCategories { get; } = new ObservableCollection<IssueCategory>();
        public async Task LoadIssueCategories()
        {
            IsBusy = true;


            try
            {


                var query = "SELECT IssueCategory.* FROM IssueTypeIssueCategoryMap JOIN IssueCategory ON IssueTypeIssueCategoryMap.IssueCategoryID = IssueCategory.ID  WHERE IssueTypeIssueCategoryMap.IssueTypeID = ?";

                var ExistinIssueCategories = LocalAppDatabase.GetRawConnection().QueryAsync<IssueCategory>(query, SelectedIssueType.ID).Result.ToList();

                /*var getItems =  LocalAppDatabase.GetRawConnection().Table<IssueType>().Where(x => x.ID == SelectedIssueType.ID).FirstOrDefaultAsync().Result.IssueCategories;*/
                lock (IssueCategories)
                {
                    IssueCategories.Clear();
                    foreach (var it in ExistinIssueCategories)
                    {
                        lock (IssueCategories)
                        {
                            IssueCategories.Add(it);
                        }
                    }
                }
            }
            catch
            {
                lock (IssueCategories)
                {
                    IssueCategories.Clear();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }


        public Command DeleteCurrentIssue { get; private set; }
        public async Task DeleteCurrentIssueTask()
        {
            SelectedStoreArea.Issues.Remove(SelectedIssue);
            StartAudit.Issues.Remove(SelectedIssue);
            Issues.Remove(SelectedIssue);
            await LocalAppDatabase.GetRawConnection().Table<Issue>().DeleteAsync(x => x.ID == SelectedIssue.ID);
            RefreshItemAreaPage();
            SelectedIssue = null;
        }

        public Command DeleteCurrentDeactInventory { get; private set; }
        public async Task DeleteCurrentDeactInventoryTask()
        {
            SelectedStoreArea.Inventories.Remove(SelectedDeactInventory);
            StartAudit.Inventories.Remove(SelectedDeactInventory);
            Inventories.Remove(SelectedDeactInventory);
            await LocalAppDatabase.GetRawConnection().Table<DeactivationInventory>().DeleteAsync(x => x.ID == SelectedDeactInventory.ID);
            RefreshItemAreaPage();
            SelectedDeactInventory = null;

        }

        [CommandTask(nameof(DeleteCurrentDeactInventoryTask))]
        public Command DeleteCurrentPedInventory { get; private set; }
        public async Task DeleteCurrentPedInventoryTask()
        {
            SelectedStoreArea.Inventories.Remove(SelectedPedInventory);
            StartAudit.Inventories.Remove(SelectedPedInventory);
            Inventories.Remove(SelectedPedInventory);
            await LocalAppDatabase.GetRawConnection().Table<PedestalInventory>().DeleteAsync(x => x.ID == SelectedPedInventory.ID);
            RefreshItemAreaPage();
            SelectedPedInventory = null;
        }

        [CommandTask(nameof(SaveIssueTask))]
        public Command SaveIssueCommand { get; private set; }
        public async Task SaveIssueTask()
        {
            SelectedIssue.IssueCategoryID = SelectedIssue.IssueCategory.ID;
            SelectedIssue.IssueTypeID = SelectedIssue.IssueType.ID;

            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedIssue);
            await Shell.Current.GoToAsync("..");
            RefreshItemAreaPage();
        }

        [CommandTask(nameof(SavePedInvTask))]
        public Command SavePedInvCommand { get; private set; }
        public async Task SavePedInvTask()
        {
            SelectedPedInventory.SystemTypeID = SelectedPedInventory.SystemType.ID;
            SelectedPedInventory.AlarmToneID = SelectedPedInventory.AlarmTone.ID;

            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedPedInventory);
            await Shell.Current.GoToAsync("..");
            RefreshItemAreaPage();
        }

        [CommandTask(nameof(SaveDeactInvTask))]
        public Command SaveDeactInvCommand { get; private set; }
        public async Task SaveDeactInvTask()
        {
            SelectedDeactInventory.DeactivatorTypeID = SelectedDeactInventory.DeactivatorType.ID;
            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedDeactInventory);
            await Shell.Current.GoToAsync("..");
            RefreshItemAreaPage();
        }

        private Command takePhoto;

        public InventoryIssueViewModelParts(INavigationService navigationService) : base(navigationService)
        {
        }

        public ICommand TakePhoto
        {
            get
            {
                if (takePhoto == null)
                {
                    takePhoto = new Command(async () => await PerformTakePhoto());
                }

                return takePhoto;
            }
        }

        private async Task PerformTakePhoto()
        {
            await Shell.Current.GoToAsync($"ViewPicturePage?ID={SelectedIssue.ID}");
        }
    }
}
