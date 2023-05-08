using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FeatureNotSupportedException = Xamarin.Essentials.FeatureNotSupportedException;
using MediaPicker = Xamarin.Essentials.MediaPicker;
using PermissionException = Xamarin.Essentials.PermissionException;
using Permissions = Xamarin.Essentials.Permissions;
using PermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class ViewPicturesViewModel : BaseViewModel
    {
        public ViewPicturesViewModel(INavigationService navigationService) : base(navigationService)
        {
            CommandTaskAttribute.InitCommands(this);
        }


        public override async Task InitializeAsync(object parameter)
        {
            issueImageExisit = false;
            if (parameter != null)
            {
                IssueId = (int)parameter;
            }
            TakePhotoAsync.Execute(null);
        }

        private bool issueImageExisit;

        private int _issueid;
        public int IssueId
        {
            get => _issueid;
            set
            {
                _issueid = value;
                LoadExistingData((int)value);

            }
        }

        public async void LoadExistingData(int v)
        {
            _issueid = v;
            //IssueImages.Clear();
            //foreach (var image in await LocalAppDatabase.GetRawConnection().Table<IssueImage>().Where(x => x.IssueID == v).ToListAsync())
            //{
            //    IssueImages.Add(image);
            //    issueImageExisit = true;
            //}

            SelectedIssueImage = null; //clear
            SelectedIssueImage = await LocalAppDatabase.GetRawConnection().Table<IssueImage>().Where(x => x.IssueID == v).FirstOrDefaultAsync();

            if (SelectedIssueImage != null)
            {
                issueImageExisit = true;
            }
        }


        //internal async Task LoadExistingData(int v)
        //{
        //    _issueid = v;
        //    IssueImages.Clear();
        //    foreach (var image in await LocalAppDatabase.GetRawConnection().Table<IssueImage>().Where(x => x.IssueID == v).ToListAsync())
        //    {
        //        IssueImages.Add(image);
        //    }
        //}

        //int _issueid;

        [CommandTask(nameof(TakePhoto))]
        public Command TakePhotoAsync { private set; get; }

        private byte[] convertByteArray(System.IO.Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task TakePhoto()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
                return;

            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                Console.WriteLine($"CapturePhotoAsync COMPLETED: {photo.FullPath}");
                var minimum = (await LocalAppDatabase.GetRawConnection().Table<IssueImage>().OrderBy(x => x.ID).FirstOrDefaultAsync())?.ID ?? 0;
                //if (minimum > -1)
                //{
                //    minimum = -1;
                //}
                //else
                //{
                //    minimum = minimum - 1;
                //}
                if (issueImageExisit)
                {
                    //minimum = IssueImages[0].ID;
                    minimum = SelectedIssueImage.ID;
                }
                else
                {
                    //new
                    minimum = minimum - 1;
                }

                var newPhoto = new IssueImage()
                {
                    Image = convertByteArray(await photo.OpenReadAsync()),
                    MimeType = photo.ContentType,
                    IssueID = _issueid,
                    ID = minimum
                };

                //IssueImages.Clear();
                //IssueImages.Add(newPhoto);
                SelectedIssueImage = newPhoto;
                await LocalAppDatabase.SaveIssueImageAsync(newPhoto, issueImageExisit);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //Feature is not supported on the device
                Debug.WriteLine($"CapturePhotoAsync THREW Feature is not supported on the device: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                //Permissions not granted
                Debug.WriteLine($"CapturePhotoAsync THREW Permissions not granted: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        // MR - There isn't a collection of images, it's only one image per issue.
        public ObservableCollection<IssueImage> IssueImages { get; } = new ObservableCollection<IssueImage>();

        //public IssueImage SelectedIssueImage = new IssueImage();

        public IssueImage _selectedIssueImage;

        public IssueImage SelectedIssueImage
        {
            get => _selectedIssueImage;
            set
            {
                _selectedIssueImage = value;
                OnPropertyChanged();
            }
        }
    }
}
