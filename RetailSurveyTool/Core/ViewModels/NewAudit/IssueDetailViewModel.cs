using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using FeatureNotSupportedException = Xamarin.Essentials.FeatureNotSupportedException;
using Permissions = Xamarin.Essentials.Permissions;
using PermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public class IssueDetailViewModel : BaseViewModel
    {

        public IssueDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override async Task InitializeAsync(object parameter)
        {
            //ImageId = (await LocalAppDatabase.GetRawConnection().Table<IssueImage>().OrderByDescending(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0;
            SelectedIssueImageList = new ObservableCollection<IssueImage>();
            // await LocalAppDatabase.GetRawConnection().Table<IssueImage>().DeleteAsync(x => x == x);

            // New Issue
            if (parameter == null)
            {
                OnLoadIssueTypesCommand();
                SelectedIssue = new Issue()
                {
                    // set default for a new issue
                    AuditID = StartAudit.ID,
                    StoreAreaID = SelectedStoreArea.ID,
                    Audit = StartAudit,
                    StoreArea = SelectedStoreArea,
                    ID = ((await LocalAppDatabase.GetRawConnection().Table<Issue>().Where(x => x.ID < 0).OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1,
                };

            }
            // Selected Issue
            else
            {
                SelectedIssue = parameter as Issue; // Selected Issue from Issue List


                OnLoadIssueTypesCommand();

                //  Set picker index for IssueTypes lookup
                var indexIssueTypes = IssueTypes.IndexOf(IssueTypes.Where(X => X.ID == SelectedIssue.IssueType.ID).FirstOrDefault());
                if (indexIssueTypes > -1)
                {
                    SelectedIssueType = IssueTypes[indexIssueTypes];


                    //  Set picker index for IssueTypes lookup
                    //OnLoadIssueCategories();
                    var indexIssueCategories = IssueCategories.IndexOf(IssueCategories.Where(X => X.ID == SelectedIssue.IssueCategory.ID).FirstOrDefault());
                    if (indexIssueCategories > -1)
                    {
                        SelectedIssueCategory = IssueCategories[indexIssueCategories];
                    }

                }
            }

            IssueId = SelectedIssue.ID;


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
            MessagingCenter.Subscribe<IssueListViewModel>(this, MessageNames.SelectedIssueMessage, (issueListViewModel) => OnResetForm());
        }

        private void OnResetForm()
        {
            SelectedIssue = null;
            SelectedIssueType = new IssueType();
            SelectedIssueCategory = new IssueCategory();

        }

        public Issue _selectedIssue;

        public Issue SelectedIssue
        {
            get => _selectedIssue;
            set
            {
                _selectedIssue = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IssueImage> _issueImage;

        public ObservableCollection<IssueImage> SelectedIssueImageList
        {
            get => _issueImage;
            set
            {
                _issueImage = value;
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
        private void OnStartAudit(Audit audit)
        {
            StartAudit = audit;
        }

        public int ImageId { get; set; }

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


        public ObservableCollection<IssueType> IssueTypes { get; } = new ObservableCollection<IssueType>();

        public ICommand LoadIssueTypesCommand => new Command(OnLoadIssueTypesCommand);

        private void OnLoadIssueTypesCommand()
        {
            // set current selected to blank
            //SelectedIssueType = null;
            //IsBusy = true;

            try
            {
                if (IssueTypes != null)
                {
                    IssueTypes.Clear();
                    //var items = LocalAppDatabase.GetIssueTypeAsync().Result;
                    var query = $"SELECT IssueType.* FROM CustomerIssueTypeMap JOIN IssueType ON CustomerIssueTypeMap.IssueTypeID = IssueType.ID  WHERE CustomerIssueTypeMap.CustomerID = {SelectedCustomer.ID} AND (IssueType.ShowForDeactivation={SelectedStoreArea.DeactivationArea} OR  IssueType.ShowForSystem ={SelectedStoreArea.PedestalArea})";

                    var issueTypes = LocalAppDatabase.GetRawConnection().QueryAsync<IssueType>(query).Result.ToList();

                    /* LocalAppDatabase.GetRawConnection().Table<IssueType>().Where
                             (x => (x.ShowForDeactivation == SelectedStoreArea.DeactivationArea || x.ShowForSystem == SelectedStoreArea.PedestalArea)
                             &&
                             (x.CustomerID == null || x.CustomerID == SelectedCustomer.ID)).ToListAsync().Result*/

                    foreach (var item in issueTypes)
                    {
                        IssueTypes.Add(item);
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
                //IsBusy = false;
            }
        }


        public IssueType _selectedIssueType;

        public IssueType SelectedIssueType
        {
            get => _selectedIssueType;
            set
            {
                if (value != null)
                {
                    _selectedIssueType = value;
                    if (value.Name != null)
                    {
                        OnPropertyChanged();
                        OnLoadIssueCategories();
                    }
                }
            }
        }


        public ICommand OnPickerSelectedIndexCchangedIssueTypeCommand => new Command(OnPickerSelectedIndexChangedIssueType);

        public ICommand DeletePhotoCommand => new Command<object>(async issueImageInputValue =>
        {
            if (issueImageInputValue != null)
            {
                IssueImage objIssueImage = issueImageInputValue as IssueImage;
                if (objIssueImage != null)
                {
                    SelectedIssueImageList.Remove(objIssueImage);
                }
            }

        });


        private void OnPickerSelectedIndexChangedIssueType()
        {

            if (SelectedIssueType != null)
            {
                SelectedIssue.IssueType = SelectedIssueType;
            }
            else
            {
                Debug.WriteLine("do nothing - SelectedIssueType - IssueDetailViewModel ");
            }
        }

        public IssueCategory _selectedIssueCategory;

        public IssueCategory SelectedIssueCategory
        {
            get => _selectedIssueCategory;
            set
            {
                if (value != null)
                {
                    _selectedIssueCategory = value;
                    if (value.Name != null)
                    {
                        OnPropertyChanged();
                    }
                }

            }
        }

        public ICommand OnPickerSelectedIndexChangedIssueCategoryCommand => new Command(OnPickerSelectedIndexChangedIssueCategory);

        private void OnPickerSelectedIndexChangedIssueCategory()
        {
            if (SelectedIssueCategory != null)
            {
                SelectedIssue.IssueCategory = SelectedIssueCategory;
            }
            else
            {
                Debug.WriteLine("do nothing - SelectedIssueCategory - IssueDetailViewModel ");
            }

        }

        public ObservableCollection<IssueCategory> IssueCategories { get; } = new ObservableCollection<IssueCategory>();

        public ICommand LoadIssueCategoriesCommand => new Command(OnLoadIssueCategories);

        private void OnLoadIssueCategories()
        {
            //IsBusy = true;


            try
            {

                var query = "SELECT IssueCategory.* FROM IssueTypeIssueCategoryMap JOIN IssueCategory ON IssueTypeIssueCategoryMap.IssueCategoryID = IssueCategory.ID  WHERE IssueTypeIssueCategoryMap.IssueTypeID = ?";

                var ExistinIssueCategories = LocalAppDatabase.GetRawConnection().QueryAsync<IssueCategory>(query, SelectedIssueType.ID).Result.ToList();

                // var getItems = new List<IssueCategory>(); //LocalAppDatabase.GetRawConnection().Table<IssueCategory>().Where(x => x.IssueTypeID == SelectedIssueType.ID).ToListAsync().Result;
                //lock (IssueCategories)
                //{
                IssueCategories.Clear();
                foreach (var it in ExistinIssueCategories)
                {
                    //lock (IssueCategories)
                    //{
                    IssueCategories.Add(it);
                    //}
                }
                //}
            }
            catch
            {
                //lock (IssueCategories)
                //{
                if (IssueCategories != null)
                {
                    IssueCategories.Clear();
                }
                //}
            }
            finally
            {
                //IsBusy = false;
            }
        }

        public ICommand SaveIssueCommand => new Command(OnSaveIssue);

        private async void OnSaveIssue()
        {
            SelectedIssue.IssueCategoryID = SelectedIssueCategory.ID;
            SelectedIssue.IssueTypeID = SelectedIssueType.ID;
            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedIssue);


            //delete previous images from db
            await LocalAppDatabase.GetRawConnection().Table<IssueImage>().DeleteAsync(x => x.IssueID == SelectedIssue.ID);

            if (SelectedIssueImageList != null)
            {
                foreach (var image in SelectedIssueImageList)
                {
                    await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(image);
                }
            }
            else
            {
                Debug.WriteLine("No Issue Image found....");
            }



            //WORKS! MessagingCenter.Send<IssueDetailViewModel, Issue>(this, MessageNames.AddIssueMessage, SelectedIssue);
            MessagingCenter.Send<IssueDetailViewModel>(this, MessageNames.RefreshIssueListMessage); //refresh list using DB
            OnResetForm();
            App.NavigationService.GoBack();
        }



        public ICommand DeleteCurrentIssueCommand => new Command(OnDeleteCurrentIssue);

        private async void OnDeleteCurrentIssue()
        {
            //SelectedStoreArea.Issues.Remove(SelectedIssue);
            //StartAudit.Issues.Remove(SelectedIssue);
            ////Issues.Remove(SelectedIssue);
            await LocalAppDatabase.GetRawConnection().Table<Issue>().DeleteAsync(x => x.ID == SelectedIssue.ID);
            await LocalAppDatabase.GetRawConnection().Table<IssueImage>().DeleteAsync(x => x.IssueID == SelectedIssue.ID);
            //RefreshItemAreaPage();
            MessagingCenter.Send<IssueDetailViewModel>(this, MessageNames.RefreshIssueListMessage); //refresh list using DB
            OnResetForm();
            App.NavigationService.GoBack();
        }

        public override bool OnBackButtonPressed()
        {
            return false;
        }


        public ICommand TakePhotoCommand => new Command(OnTakePhoto);

        /// <summary>
        // started MD Jewel Devlopment..........
        /// </summary>

        private bool issueImageExisit;
        private int _issueid;

        public bool IssueImageExisit
        {
            get => issueImageExisit;
            set
            {
                issueImageExisit = value;
                OnPropertyChanged();
            }
        }


        public int IssueId
        {
            get => _issueid;
            set
            {
                _issueid = value;
                LoadExistingData((int)value);

            }
        }

        public async void OnTakePhoto()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
                return;

            try
            {
                var photo = await Microsoft.Maui.Media.MediaPicker.CapturePhotoAsync();

                Console.WriteLine($"CapturePhotoAsync COMPLETED: {photo.FullPath}");
                //var minimum = (await LocalAppDatabase.GetRawConnection().Table<IssueImage>().OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 1;

                ImageId = ((await LocalAppDatabase.GetRawConnection().Table<IssueImage>().Where(x => x.ID < 0).OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0) - 1;

                var newPhoto = new IssueImage()
                {
                    Image = convertByteArray(await photo.OpenReadAsync()),
                    MimeType = photo.ContentType,
                    IssueID = _issueid,
                    ID = ImageId
                };

                Debug.WriteLine($"Image Path: {photo.FullPath}");
                File.Delete(photo.FullPath);


                //SelectedIssueImageList.Clear();
                SelectedIssueImageList.Add(newPhoto);

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //Feature is not supported on the device
                Debug.WriteLine($"CapturePhotoAsync THREW Feature is not supported on the device: {fnsEx.Message}");
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                //Permissions not granted
                Debug.WriteLine($"CapturePhotoAsync THREW Permissions not granted: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }


        private byte[] convertByteArray(System.IO.Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }




        public async void LoadExistingData(int v)
        {
            foreach (var row in await LocalAppDatabase.GetRawConnection().Table<IssueImage>().Where(x => x.IssueID == v).ToListAsync())
            {
                SelectedIssueImageList.Add(row);
            }
        }

        // App.NavigationService.NavigateTo("ViewPicturesPage", SelectedIssue.ID);


        //public ICommand TakePhotoCommand => new Command(TakePhoto);

        //private Command takePhoto;

        //public ICommand TakePhoto
        //{
        //    get
        //    {
        //        if (takePhoto == null)
        //        {
        //            takePhoto = new Command(async () => await PerformTakePhoto());
        //        }

        //        return takePhoto;
        //    }
        //}

        //private async Task PerformTakePhoto()
        //{
        //    await Shell.Current.GoToAsync($"ViewPicturesPage?ID={SelectedIssue.ID}");
        //}

    }
}
