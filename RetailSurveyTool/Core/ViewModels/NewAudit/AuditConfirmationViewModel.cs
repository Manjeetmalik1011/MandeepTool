using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using Jci.RetailSurveyTool.TechnicianApp.Views;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Linq;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public class AuditConfirmationViewModel : BaseViewModel
    {
        public AuditConfirmationViewModel(INavigationService navigationService) : base(navigationService)
        {
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, Audit>(this, MessageNames.AuditComplete, (auditStoreAreaListViewModel, audit) => OnCustomerChanged(audit));

        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, Audit>(this, MessageNames.AuditComplete, (auditStoreAreaListViewModel, audit) => OnCustomerChanged(audit));

        }

        public ICommand AuditCompleteCommand => new Command(AuditComplete);

        public Audit _selectedAudit { get; set; }


        public Audit SelectedAudit
        {
            get => _selectedAudit;
            set
            {
                _selectedAudit = value;
                OnPropertyChanged();
            }
        }

        public void OnCustomerChanged(Audit audit)
        {
            _selectedAudit = audit;

        }

        private async void AuditComplete()
        {
            /*var answer = await App.Current.MainPage.DisplayAlert("Confirmation !", "Are you sure you want to Complete the Audit?", "Yes", "No");

            if (answer)
            {
                
            }*/

            SelectedAudit.Completed = DateTime.UtcNow;
            SelectedAudit.Status = "Completed";
            await LocalAppDatabase.GetRawConnection().InsertOrReplaceAsync(SelectedAudit);

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
        }




    }
}
